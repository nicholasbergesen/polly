using Newtonsoft.Json;
using Polly.Data;
using Polly.Domain;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Polly.ConsoleNet
{
    public class Refresh : SimpleWorker
    {
        readonly IDownloader _downloader;
        readonly object _lock = new object();

        public Refresh(IDownloader downloader)
            :base()
        {
            ServicePointManager.DefaultConnectionLimit = 150;
            _downloader = downloader;
        }

        protected override async Task DoWorkInternalAsync(CancellationToken token)
        {
            var startTime = DateTime.Now;
            int count = 0;
            var toDo = new BlockingCollection<ProductDownload>(new ConcurrentQueue<ProductDownload>(), 5000);
            var total = 0;
            List<Task> tasks = new List<Task>();

            //feeder
            tasks.Add(Task.Run(async () =>
            {

                while (!toDo.IsAddingCompleted)
                {
                    var newItems = await DataAccess.GetRefreshItemsAsync()
                        .ConfigureAwait(false);

                    foreach (var item in newItems)
                    {
                        toDo.Add(item);
                        total++;
                    }

                    if (newItems.Count < 100)
                    {
                        toDo.CompleteAdding();
                        Console.WriteLine();
                        Console.WriteLine("All Items up to date. Adding completed.");
                    }


                    while (toDo.Count > 1000)
                        await Task.Delay(2000)
                            .ConfigureAwait(false);
                }
            }));

            //consumers
            for (int i = 0; i < 30; i++)
            {
                tasks.Add(Task.Run(async () =>
                {
                    while (!toDo.IsCompleted)
                    {
                        if (!toDo.TryTake(out ProductDownload productDownload, 100))
                            continue;

                        try
                        {
                            //81976 with null url but has unique code.
                            if (string.IsNullOrWhiteSpace(productDownload.Url))
                            {
                                ++count;
                                await DataAccess.UpdateLastChecked(productDownload.Id, DateTime.Now)
                                    .ConfigureAwait(false);
                                continue;
                            }

                            var httpResponse = await _downloader.DownloadAsync(BuildDownloadUrl(productDownload.Url));
                            if (string.IsNullOrWhiteSpace(httpResponse))
                            {
                                ++count;
                                await DataAccess.UpdateLastChecked(productDownload.Id, DateTime.Now)
                                    .ConfigureAwait(false);
                                continue;
                            }

                            TakealotJson jsonObject = JsonConvert.DeserializeObject<TakealotJson>(httpResponse);
                            await SaveNewPrice(jsonObject, productDownload)
                                .ConfigureAwait(false);

                            lock (_lock)
                            {
                                RaiseOnProgress(++count, total, startTime);
                            }
                        }
                        catch (Exception e)
                        {
                            ++count;
                            await DataAccess.UpdateLastChecked(productDownload.Id, DateTime.Now)
                                .ConfigureAwait(false);
                            Console.WriteLine(e);
                            Console.ReadLine();
                        }
                    }
                })
                .ContinueWith(ctask =>
                {
                    lock (_lock)
                    {
                        Console.WriteLine();
                        Console.WriteLine($"A task ended unexpectedly:{ctask.Exception.Message}");
                    }
                },
                TaskContinuationOptions.OnlyOnFaulted));
            }

            await Task.WhenAll(tasks);
        }

        private const string TakealotApi = "https://api.takealot.com/rest/v-1-8-0/product-details";

        private string BuildDownloadUrl(string loc)
        {
            int lastindex = loc.LastIndexOf('/');
            return string.Concat(TakealotApi, loc.Substring(lastindex, loc.Length - lastindex), "?platform=desktop");
        }

        private async Task SaveNewPrice(TakealotJson jsonObject, ProductDownload productDownload)
        {
            //no change, update last checked
            await DataAccess.UpdateLastChecked(productDownload.Id, jsonObject.meta.date_retrieved)
                .ConfigureAwait(false);

            bool hasPurchasePrice = !jsonObject.event_data.documents.product.purchase_price.HasValue;
            if (hasPurchasePrice)
                return;

            decimal price = jsonObject.event_data.documents.product.purchase_price.Value;
            decimal? originalPrice = jsonObject.event_data.documents.product.original_price;

            if (price >= originalPrice)//prevent bad data
                originalPrice = null;

            if (productDownload.Price != price)
            {
                await DataAccess.SaveAsync(new PriceHistory(productDownload.Price, productDownload.PriceId, price, originalPrice) { ProductId = productDownload.Id })
                    .ConfigureAwait(false);
                return;
            }
        }

        public override string ToString()
        {
            return "Refresh";
        }
    }
}

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
    public class Upload : SimpleWorker
    {
        readonly IDownloader _downloader;
        readonly object _lock = new object();
        readonly object _writelock = new object();

        public Upload(IDownloader downloader)
        {
            ServicePointManager.DefaultConnectionLimit = 150;
            _downloader = downloader;
        }

        protected override async Task DoWorkInternalAsync(CancellationToken token)
        {
            var startTime = DateTime.Now;
            int count = 0;
            var doneUrls = await GetDone();
            var toDo = await GetToDo(doneUrls);
            var total = toDo.Count;
            StreamWriter sw = new StreamWriter("done.txt", append: true);
            List<Task> tasks = new List<Task>();

            for (int i = 0; i < 15; i++)
            {
                tasks.Add(Task.Run(async () =>
                {
                    while (toDo.TryDequeue(out string line))
                    {
                        var items = line.Split(',');
                        var url = items[1];
                        var httpResponse = await _downloader.DownloadAsync(url);
                        if (string.IsNullOrWhiteSpace(httpResponse))
                        {
                            lock (_writelock)
                            {
                                sw.WriteLine(url);
                            }
                            continue;
                        }

                        try
                        {
                            TakealotJson jsonObject = JsonConvert.DeserializeObject<TakealotJson>(httpResponse);

                            try
                            {
                                await SaveProductFromJsonObject(jsonObject);
                                lock (_writelock)
                                {
                                    sw.WriteLine(url);
                                }
                            }
                            catch
                            {
                                await SaveProductFromJsonObject(jsonObject);
                                lock (_writelock)
                                {
                                    sw.WriteLine(url);
                                }
                            }
                        }
                        catch (System.Data.Entity.Core.EntityException)
                        {
                            toDo.Enqueue(line);
                        }
                        catch (Exception)
                        {
                            await sw.WriteLineAsync(url);
                        }
                        Interlocked.Increment(ref count);
                        RaiseOnProgress(count, total, startTime);
                    }
                    count++;
                })
                .ContinueWith(ctask =>
                {
                    lock (_lock)
                    {
                        Console.WriteLine();
                        Console.WriteLine($"A task ended unexpectedly:{ctask.Exception.Message}");
                    }
                },
                TaskContinuationOptions.OnlyOnFaulted)
                .ContinueWith(ctask =>
                {
                },
                TaskContinuationOptions.OnlyOnCanceled));
            }
            try
            {
                await Task.WhenAll(tasks);
            }
            finally
            {
                sw.Close();
                sw.Dispose();
            }
        }

        private async Task<ConcurrentQueue<string>> GetToDo(HashSet<string> done)
        {
            HashSet<string> toDoUrls = new HashSet<string>();

            using (StreamReader sr = new StreamReader("downloadLinks.txt"))
            {
                while (!sr.EndOfStream)
                {
                    var line = await sr.ReadLineAsync();
                    var url = line.Split(',')[1];
                    if (done.Contains(url))
                        continue;
                    toDoUrls.Add(line);
                }
            }
            return new ConcurrentQueue<string>(toDoUrls);
        }

        private async Task<HashSet<string>> GetDone()
        {
            HashSet<string> doneUrls = new HashSet<string>();
            if (File.Exists("done.txt"))
                using (StreamReader sr = new StreamReader("done.txt"))
                {
                    while (!sr.EndOfStream)
                    {
                        doneUrls.Add(await sr.ReadLineAsync());
                    }
                }
            return doneUrls;
        }

        private async Task<Data.Product> SaveProductFromJsonObject(TakealotJson jsonObject)
        {
            bool hasPurchasePrice = !jsonObject.event_data.documents.product.purchase_price.HasValue;
            if (hasPurchasePrice)
                return null;

            decimal price = jsonObject.event_data.documents.product.purchase_price.Value;
            decimal? originalPrice = jsonObject.event_data.documents.product.original_price;

            if (price >= originalPrice)//prevent bad data
                originalPrice = null;

            Data.Product product = await DataAccess.FetchProductOrDefault(jsonObject.data_layer.prodid);
            if (product != null)
            {
                var lastPrice = await DataAccess.FetchProductLastPrice(product.Id);
                if (lastPrice.Price == price)
                    return null;
                await DataAccess.SaveAsync(new PriceHistory(lastPrice, price, originalPrice) { ProductId = product.Id });
                return null;
            }

            product = new Data.Product()
            {
                UniqueIdentifier = jsonObject.data_layer.prodid
            };
            product.PriceHistory.Add(new PriceHistory(null, price, originalPrice) { ProductId = product.Id });

            product.Breadcrumb = jsonObject.breadcrumbs?.items.Select(x => x.name).Aggregate((i, j) => i + "," + j);
            product.Title = jsonObject.title;
            product.Description = jsonObject.description?.html;
            product.Category = jsonObject.data_layer.categoryname?.Select(x => x).Aggregate((i, j) => i + "," + j);
            if (jsonObject.gallery.images.Any())
                product.Image = jsonObject.gallery.images[0].Replace("{size}", "pdpxl");
            product.Url = jsonObject.desktop_href;
            product.LastChecked = jsonObject.meta.date_retrieved;

            await DataAccess.SaveAsync(product);

            return product;
        }

        public override string ToString()
        {
            return "Upload";
        }
    }
}

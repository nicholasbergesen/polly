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
    public class Upload : AsyncWorkerBase
    {
        IDownloader _downloader;
        HttpClient _httpClient = new HttpClient();
        readonly object _lock = new object();

        public Upload(IDownloader downloader)
        {
            ServicePointManager.DefaultConnectionLimit = 150;
            _downloader = downloader;
            OnProgress += Upload_OnProgress;
            OnStart += Upload_OnStart;
        }

        private void Upload_OnStart(object sender, EventArgs e)
        {
            Console.Clear();
        }

        private void Upload_OnProgress(object sender, ProgressEventArgs e)
        {
            Console.CursorTop = 0;
            Console.CursorLeft = 0;
            Console.WriteLine(e.ProgressString);
        }

        protected override async Task DoWorkInternalAsync(CancellationToken token)
        {
            var startTime = DateTime.Now;
            int count = 0;
            var doneUrls = await GetDone();
            var toDo = await GetToDo(doneUrls);
            var total = toDo.Count;

            List<Task> tasks = new List<Task>();

            for (int i = 0; i < 30; i++)
            {
                tasks.Add(Task.Run(async () =>
                {
                    try
                    {
                        while (toDo.TryDequeue(out string line))
                        {
                            var items = line.Split(',');
                            var url = items[1];
                            var httpResponse = await _downloader.DownloadAsync(url);
                            if (string.IsNullOrWhiteSpace(httpResponse))
                            {
                                lock (_lock)
                                {
                                    Write(url);
                                }
                                continue;
                            }
                            TakealotJson jsonObject = JsonConvert.DeserializeObject<TakealotJson>(httpResponse);
                            await SaveProductFromJsonObject(jsonObject);

                            lock (_lock)
                            {
                                Write(url);
                            }
                            RaiseOnProgress(++count, total, startTime);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        Console.ReadLine();
                    }
                }));
            }

            await Task.WhenAll(tasks);
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

        private void Write(string line)
        {
            using (StreamWriter sw = new StreamWriter("done.txt", append: true))
            {
                sw.WriteLine(line);
            }
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

            Data.Product product = DataAccess.FetchProductOrDefault(jsonObject.data_layer.prodid);
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

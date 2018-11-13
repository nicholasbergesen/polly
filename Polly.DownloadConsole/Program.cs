using Polly.Data;
using RobotsSharpParser;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Polly.DownloadConsole
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.CancelKeyPress += Console_CancelKeyPress;
            ServicePointManager.DefaultConnectionLimit = 200;
            int websiteId = int.Parse(Console.ReadLine());
            Download(websiteId).Wait();
            Console.ReadLine();
        }

        static CancellationTokenSource source = new CancellationTokenSource();

        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            source.Cancel();
            e.Cancel = true;
        }

        static CancellationToken token = source.Token;

        private static async Task Download(int websiteId)
        {
            var website = DataAccess.GetWebsiteById(websiteId);
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("User-Agent", website.UserAgent);
            int threads = 16;
            Task[] tasks = new Task[threads];
            DateTime startTime = DateTime.Now;
            Console.WriteLine($"[{DateTime.Now}] Started");
            var downloadQueueIds = DataAccess.GetDownloadQueueIds(website.Id);

            int totalSize = downloadQueueIds.Count;

            for (int i = 0; i < threads; i++)
            {
                Task newTask = Task.Run(async () =>
                {
                    while (downloadQueueIds.TryDequeue(out long nextQueueId) && !token.IsCancellationRequested)
                    {
                        var downloadUrl = await DataAccess.GetDownloadQueueItemByIdAsync(nextQueueId);
                        string html;
                        var response = await httpClient.GetAsync(downloadUrl);
                        if (response.IsSuccessStatusCode)
                            html = await response.Content.ReadAsStringAsync();
                        else
                        {
                            await DataAccess.DeleteAsync(nextQueueId);
                            continue;
                        }

                        await SaveProductFromJson(html);
                        await DataAccess.DeleteAsync(nextQueueId);
                    }
                }, token);
                tasks[i] = newTask;
            }

            var runningTasks = Task.WhenAll(tasks);

            while (!runningTasks.IsCompleted)
            {
                WriteOutput(RaiseOnProgress(totalSize - downloadQueueIds.Count, totalSize, startTime));
                await Task.Delay(1000);
            }

            await runningTasks;

            Console.WriteLine($"[{DateTime.Now}] Stopped");
            Console.WriteLine($"Press any key to close console.");
        }

        private static object _lock = new object();

        public static string RaiseOnProgress(int requestCount, int totalSize, DateTime startTime)
        {
            double downloadRate = Math.Max(requestCount / Math.Max(DateTime.Now.Subtract(startTime).TotalSeconds, 1), 1);
            int itemsRemaining = totalSize - requestCount;
            return $"{requestCount} of {totalSize} { (requestCount * 1.00 / totalSize * 1.00 * 100):0.####}% { downloadRate:0.##}/s ETA:{ DateTime.Now.AddSeconds(itemsRemaining / downloadRate) }        ";
        }

        private static void WriteOutput(string output)
        {
            Console.CursorLeft = 0;
            Console.CursorTop = Math.Max(Console.CursorTop - 1, 0);
            Console.WriteLine(output);
        }

        private static async Task SaveProductFromJson(string httpResponse)
        {
            if (string.IsNullOrEmpty(httpResponse))
                return;

            TakealotJson jsonObject = JsonConvert.DeserializeObject<TakealotJson>(httpResponse);
            bool hasPurchasePrice = !jsonObject.event_data.documents.product.purchase_price.HasValue;
            if (hasPurchasePrice)
                return;

            decimal price = jsonObject.event_data.documents.product.purchase_price.Value;
            decimal? originalPrice = jsonObject.event_data.documents.product.original_price;

            if (price >= originalPrice)//prevent bad data
                originalPrice = null;

            Data.Product product = DataAccess.FetchProductOrDefault(jsonObject.data_layer.prodid);
            if (product != null)
            {
                var lastPrice = await DataAccess.FetchProductLastPrice(product.Id);
                if (lastPrice.Price == price)
                    return;
                await DataAccess.SaveAsync(new PriceHistory(lastPrice, price, originalPrice) { ProductId = product.Id });
                return;
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
        }
    }
}


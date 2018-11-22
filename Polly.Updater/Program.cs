using Newtonsoft.Json;
using Polly.Data;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Polly.Updater
{
    class Program
    {
        private const string TakealotApi = "https://api.takealot.com/rest/v-1-8-0/product-details";

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
            int threads = 8;
            Task[] tasks = new Task[threads];
            DateTime startTime = DateTime.Now;
            Console.WriteLine($"[{DateTime.Now}] Started");
            var productIds = DataAccess.GetDownloadQueueProductIds();

            int totalSize = productIds.Count;

            for (int i = 0; i < threads; i++)
            {
                Task newTask = Task.Run(async () =>
                {
                    while (productIds.TryDequeue(out long nextProductId) && !token.IsCancellationRequested)
                    {
                        var product = await DataAccess.FetchProductOrDefault(nextProductId);

                        product.LastChecked = DateTime.Now;
                        await DataAccess.SaveAsync(product);

                        var downloadUrl = BuildDownloadUrl(product.Url);
                        string html;
                        var response = await httpClient.GetAsync(downloadUrl);
                        if (response.IsSuccessStatusCode)
                            html = await response.Content.ReadAsStringAsync();
                        else
                            continue;

                        await SaveProductFromJson(html, product);
                    }
                }, token);
                tasks[i] = newTask;
            }

            var runningTasks = Task.WhenAll(tasks);

            while (!runningTasks.IsCompleted)
            {
                WriteOutput(RaiseOnProgress(totalSize - productIds.Count, totalSize, startTime));
                await Task.Delay(1000);
            }

            await runningTasks;

            Console.WriteLine($"[{DateTime.Now}] Stopped");
            Console.WriteLine($"Press any key to close console.");
        }

        protected static string BuildDownloadUrl(string productUrl)
        {
            int lastindex = productUrl.LastIndexOf('/');
            return string.Concat(TakealotApi, productUrl.Substring(lastindex, productUrl.Length - lastindex), "?platform=desktop");
        }

        private static readonly object _lock = new object();

        public static string RaiseOnProgress(int requestCount, int totalSize, DateTime startTime)
        {
            double downloadRate = Math.Max(requestCount / Math.Max(DateTime.Now.Subtract(startTime).TotalSeconds, 1), 1);
            int itemsRemaining = totalSize - requestCount;
            if (downloadRate > 200)
                throw new Exception("They got me");
            return $"{requestCount} of {totalSize} { (requestCount * 1.00 / totalSize * 1.00 * 100):0.####}% { downloadRate:0.##}/s ETA:{ DateTime.Now.AddSeconds(itemsRemaining / downloadRate) }        ";
        }

        private static void WriteOutput(string output)
        {
            Console.CursorLeft = 0;
            Console.CursorTop = Math.Max(Console.CursorTop - 1, 0);
            Console.WriteLine(output);
        }

        private static async Task SaveProductFromJson(string httpResponse, Data.Product product)
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

            var lastPrice = product.PriceHistory.Last();
            if (lastPrice.Price == price)
                return;
            else
                await DataAccess.SaveAsync(new PriceHistory(lastPrice, price, originalPrice) { ProductId = product.Id });
        }
    }
}

using HtmlAgilityPack;
using Newtonsoft.Json;
using Polly.Data;
using Polly.Downloader;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using Product = Polly.Data.Product;

namespace Polly.ProcessConsole
{
    public class Program
    {
        private static object _lock = new object();

        static CancellationTokenSource source = new CancellationTokenSource();

        static void Main(string[] args)
        {
            Console.CancelKeyPress += Console_CancelKeyPress;
            var startTime = DateTime.Now;
            Console.WriteLine($"Started at {startTime}");
            int threads = 4;
            Thread[] activeThreads = new Thread[threads];
            var dataIds = DataAccess.GetDownloadDataIdsAsync(1);
            int totalSize = dataIds.Count;
            var token = source.Token;
            for (int i = 0; i < threads; i++)
            {
                Thread newThread = new Thread(new ThreadStart(() =>
                {
                    DownloadData downloadData;

                    while (dataIds.TryDequeue(out long nextQueueId) && !token.IsCancellationRequested)
                    {
                        try
                        {
                            downloadData = DataAccess.GetNextDownloadData(nextQueueId);
                            Product product;
                            if (downloadData.Website.DataSourceTypeId == DataSourceTypeEnum.Html)
                            {
                                product = ProcessHtml(downloadData);
                            }
                            else if (downloadData.Website.DataSourceTypeId == DataSourceTypeEnum.JSON)
                            {
                                product = ProcessJson(downloadData);
                            }
                            else
                            {
                                throw new Exception("Unknown dataSourceType");
                            }

                            if (product != null) //price is the same
                                DataAccess.SaveAsync(product);
                            DataAccess.DeleteAsync(downloadData);
                        }
                        catch(Exception)
                        { }
                    }
                }));
                activeThreads[i] = newThread;
                activeThreads[i].Start();
            }

            while (activeThreads.Any(x => x.IsAlive) && !token.IsCancellationRequested)
            {
                Console.CursorLeft = 0;
                Console.CursorTop = 1;
                var unprocessedCount = DataAccess.UnprocessedCount();
                Console.WriteLine(RaiseOnProgress(totalSize - unprocessedCount, totalSize, startTime));
                Thread.Sleep(2000);
            }
            Console.WriteLine($"[{DateTime.Now}] Stopped");
            Console.WriteLine($"Press any key to close console.");
            Console.ReadLine();
        }

        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            source.Cancel();
            e.Cancel = true;
        }

        public static string RaiseOnProgress(int requestCount, int totalSize, DateTime startTime)
        {
            double downloadRate = Math.Max(requestCount / Math.Max(DateTime.Now.Subtract(startTime).TotalSeconds, 1), 1);
            int itemsRemaining = totalSize - requestCount;
            return $"{requestCount} of {totalSize} { (requestCount * 1.00 / totalSize * 1.00 * 100):0.####}% { downloadRate:0.##}/s ETA:{ DateTime.Now.AddSeconds(itemsRemaining / downloadRate) }        ";
        }

        private static Product ProcessHtml(DownloadData downloadData)
        {
            Product product = new Product();
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(downloadData.RawHtml);
            product.Title = htmlDocument.DocumentNode.SelectSingleNode(downloadData.Website.HeadingXPath).InnerText.Trim();
            product.Description = htmlDocument.DocumentNode.SelectSingleNode(downloadData.Website.DescriptionXPath).InnerText.Trim();
            if (decimal.TryParse(htmlDocument.DocumentNode.SelectSingleNode(downloadData.Website.PriceXPath).InnerText.Trim().Replace(",", ""), out decimal res))
                product.PriceHistory.Add(new PriceHistory(res));
            product.UniqueIdentifier = downloadData.Url;
            if (downloadData.Website.BreadcrumbXPath != null) product.Breadcrumb = htmlDocument.DocumentNode.SelectSingleNode(downloadData.Website.BreadcrumbXPath)?.InnerText.Trim();
            if (downloadData.Website.CategoryXPath != null) product.Category = htmlDocument.DocumentNode.SelectSingleNode(downloadData.Website.CategoryXPath)?.InnerText.Trim();
            return product;
        }

        private static Product ProcessJson(DownloadData downloadData)
        {
            TakealotJson jsonObject = JsonConvert.DeserializeObject<TakealotJson>(downloadData.RawHtml);
            Product product = DataAccess.FetchProductOrDefault(jsonObject.data_layer.prodid);
            var lastPrice = product.PriceHistory.LastOrDefault();
            if (lastPrice != null)
            {
                if (!jsonObject.data_layer.totalPrice.HasValue || (lastPrice.Price == jsonObject.data_layer.totalPrice
                    && lastPrice.OriginalPrice == jsonObject.buybox.listing_price))
                    return null;
                else
                {
                    var priceHistory = new PriceHistory(jsonObject.data_layer.totalPrice.Value, jsonObject.buybox.listing_price);
                    priceHistory.ProductId = product.Id;
                    product.PriceHistory.Add(priceHistory);
                }
            }
            else
                product.PriceHistory.Add(new PriceHistory(jsonObject.data_layer.totalPrice.Value, jsonObject.buybox.listing_price));

            product.Breadcrumb = jsonObject.breadcrumbs?.items.Select(x => x.name).Aggregate((i, j) => i + ", " + j);
            product.Title = jsonObject.title;
            product.Description = jsonObject.description?.html;
            product.Category = jsonObject.data_layer.categoryname?.Select(x => x).Aggregate((i, j) => i + ", " + j);
            product.Image = jsonObject.gallery.images[0].Replace("{size}", "pdpxl");
            product.Url = jsonObject.desktop_href;
            product.LastChecked = DateTime.Now;

            return product;
        }
    }
}

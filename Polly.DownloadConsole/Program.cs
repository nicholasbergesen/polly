using Polly.Data;
using RobotsSharpParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Polly.DownloadConsole
{
    public class Program
    {
        static void Main(string[] args)
        {
            ServicePointManager.DefaultConnectionLimit = 100;
            int websiteId = int.Parse(Console.ReadLine());
            Download(websiteId).Wait();
            Console.ReadLine();
        }

        private static async Task Download(int websiteId)
        {
            try
            {
                var website = DataAccess.GetWebsiteById(websiteId);

                HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("User-Agent", website.UserAgent);
                int crawlDelay = GetCrawlDelay(website);

                DateTime startTime = DateTime.Now;
                Console.WriteLine($"[{DateTime.Now}] Started");

                List<Task> activeTasks = new List<Task>(Environment.ProcessorCount);
                int batchSize = 10000;
                int batchPosition = 111608;
                int requestCount = 111608;
                var downloadBatch = await DataAccess.GetDownloadQueueBatchAsync(website.Id, batchSize, batchPosition);
                int totalSize = await DataAccess.DownloadQueueCountAsync(website.Id);

                while (downloadBatch.Count > 0)
                {
                    var next = downloadBatch.FirstOrDefault();
                    downloadBatch.Remove(next);
                    //await Task.Delay(crawlDelay);
                    var downloadTask = httpClient.GetStringAsync(next.DownloadUrl);
                    activeTasks.Add(HandleDownloadTask(downloadTask, next.DownloadUrl, website.Id));

                    if (activeTasks.Count >= Environment.ProcessorCount)
                    {
                        //await Task.Delay(crawlDelay);
                        var whenallTask = Task.WhenAll(activeTasks.Where(x => x != null).ToArray());
                        activeTasks.Clear();
                    }

                    if (downloadBatch.Count == 0)
                    {
                        batchPosition += batchSize;
                        downloadBatch = await DataAccess.GetDownloadQueueBatchAsync(website.Id, batchSize, batchPosition);
                    }
                    WriteOutput(RaiseOnProgress(requestCount++, totalSize, startTime));
                }

                Console.WriteLine($"[{DateTime.Now}] Stopped");
                Console.WriteLine($"Press any key to close console.");
            }
            catch (Exception)
            { }
        }

        private static async Task HandleDownloadTask(Task<string> downloadTask, string downloadUrl, long websiteId)
        {
            string html;
            try
            {
                html = await downloadTask;
            }
            catch
            {
                html = null;
            }
            var downloadData = new DownloadData()
            {
                RawHtml = html,
                Url = downloadUrl,
                WebsiteId = websiteId,
            };
            await DataAccess.SaveAsync(downloadData);
        }

        public static string RaiseOnProgress(int requestCount, int totalSize, DateTime startTime)
        {
            double downloadRate = Math.Max(requestCount / Math.Max(DateTime.Now.Subtract(startTime).TotalSeconds, 1), 1);
            int itemsRemaining = totalSize - requestCount;
            return $"{requestCount} of {totalSize} { (requestCount * 1.00 / totalSize * 1.00):0.####}% { downloadRate:0.##}/s ETA:{ DateTime.Now.AddSeconds(itemsRemaining / downloadRate) }        ";
        }

        private static void WriteOutput(string output)
        {
            Console.CursorLeft = 0;
            Console.CursorTop = Math.Max(Console.CursorTop - 1, 0);
            Console.WriteLine(output);
        }

        private static int GetCrawlDelay(Website website)
        {
            Robots robots = new Robots(website.Domain, website.UserAgent, enableErrorCorrection: true);
            robots.Load();
            int crawlDelay = robots.GetCrawlDelay();

            if (crawlDelay == default(int))
                crawlDelay = 100; //10/s second default
            else if (crawlDelay < 10)
                crawlDelay = crawlDelay * 1000;
            return crawlDelay;
        }

        //public static byte[] Compress(byte[] inputData)
        //{
        //    using (var compressIntoMemoryStream = new MemoryStream())
        //    {
        //        using (var gzs = new GZipStream(compressIntoMemoryStream, CompressionMode.Compress))
        //        {
        //            gzs.Write(inputData, 0, inputData.Length);
        //        }
        //        return compressIntoMemoryStream.ToArray();
        //    }
        //}

        //public static byte[] Decompress(byte[] inputData)
        //{
        //    using (var compressedMemoryStream = new MemoryStream(inputData))
        //    {
        //        using (var decompressedMs = new MemoryStream())
        //        {
        //            using (var gzs = new GZipStream(compressedMemoryStream, CompressionMode.Decompress))
        //            {
        //                gzs.CopyTo(decompressedMs);
        //            }
        //            return decompressedMs.ToArray();
        //        }
        //    }
        //}
    }
}


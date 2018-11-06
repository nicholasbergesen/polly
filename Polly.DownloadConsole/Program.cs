using Polly.Data;
using RobotsSharpParser;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Linq;

namespace Polly.DownloadConsole
{
    public class Program
    {
        static void Main(string[] args)
        {
            ServicePointManager.DefaultConnectionLimit = 100;
            int websiteId = int.Parse(Console.ReadLine());
            Download(websiteId);
            Console.WriteLine("Done");
            Console.ReadLine();
        }

        private static void Download(int websiteId)
        {
            var website = DataAccess.GetWebsiteById(websiteId);
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("User-Agent", website.UserAgent);
            int crawlDelay = GetCrawlDelay(website);
            int threads = 20;// Environment.ProcessorCount;
            Thread[] activeThreads = new Thread[threads];
            DateTime startTime = DateTime.Now;
            Console.WriteLine($"[{DateTime.Now}] Started");
            var downloadQueueIds = DataAccess.GetDownloadQueueIdsAsync(website.Id);

            int totalSize = downloadQueueIds.Count;

            for (int i = 0; i < threads; i++)
            {
                Thread newThread = new Thread(new ThreadStart(() =>
                {
                    try
                    {
                        while (downloadQueueIds.TryDequeue(out long nextQueueId))
                        {
                            var downloadQueueItem = DataAccess.GetDownloadQueueItemByIdAsync(nextQueueId);
                            string html;
                            try
                            {
                                html = httpClient.GetStringAsync(downloadQueueItem.DownloadUrl).Result;
                            }
                            catch
                            {
                                DataAccess.DeleteAsync(nextQueueId);
                                continue;
                            }
                            var downloadData = new DownloadData()
                            {
                                RawHtml = html,
                                Url = downloadQueueItem.DownloadUrl,
                                WebsiteId = websiteId,
                            };
                            DataAccess.SaveAsync(downloadData);
                            DataAccess.DeleteAsync(downloadQueueItem);

                            if (downloadQueueIds.IsEmpty)
                            {
                                lock (_lock)
                                {
                                    if (downloadQueueIds.IsEmpty)
                                    {
                                        downloadQueueIds = DataAccess.GetDownloadQueueIdsAsync(website.Id);
                                        startTime = DateTime.Now;
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    { }
                }));
                activeThreads[i] = newThread;
                activeThreads[i].Start();
            }

            while (activeThreads.Any(x => x.IsAlive))
            {
                WriteOutput(RaiseOnProgress(totalSize - downloadQueueIds.Count, totalSize, startTime));
                Thread.Sleep(1000);
            }
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


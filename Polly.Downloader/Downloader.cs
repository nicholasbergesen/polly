using Polly.Data;
using RobotsSharpParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Polly.Downloader
{
    public abstract class Downloader
    {
        private Thread _mainDownloadThread;
        protected Website Website;

        public event EventHandler OnStart;
        public event EventHandler OnEnd;
        public event ProgressEventHandler OnProgress;

        public delegate void ProgressEventHandler(object sender, ProgressEventArgs e);

        public Downloader(Website website)
        {
            Website = website;
            _mainDownloadThread = new Thread(new ThreadStart(ThreadAction));
            _mainDownloadThread.Name = nameof(_mainDownloadThread);
        }

        public void Start()
        {
            _mainDownloadThread.Start();
        }

        public bool IsAlive => _mainDownloadThread.IsAlive;

        private void ThreadAction()
        {
            RaiseOnStart();
            Download();
            RaiseOnEnd();
        }

        private void Download()
        {
            Robots robots = new Robots(Website.Domain, Website.UserAgent, enableErrorCorrection: true);
            robots.Load();
            int crawlDelay = robots.GetCrawlDelay();
            var websiteLinksToDownload = robots.GetSitemapLinks();

            var filteredList = websiteLinksToDownload.Where(FilterProducts()).ToList(); 

            if (crawlDelay == default(int))
                crawlDelay = 100; //10/s second default
            else if (crawlDelay < 10)
                crawlDelay = crawlDelay * 1000;

            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("User-Agent", Website.UserAgent);

            int activeRequestCounter = 0;
            int totalRequestCount = 0;

            DateTime startTime = DateTime.Now;

            foreach (tUrl websiteLink in filteredList)
            {
                activeRequestCounter++;
                Task.Run(async () =>
                {
                    try
                    {
                        string downloadUrl = BuildDownloadUrl(websiteLink.loc);
                        var downloadData = new DownloadData()
                        {
                            Url = downloadUrl,
                            WebsiteId = Website.Id
                        };

                        downloadData.RawHtml = await httpClient.GetStringAsync(downloadUrl);
                        await DataAccess.SaveAsync(downloadData);
                    }
                    finally
                    {
                        activeRequestCounter--;
                    }
                });

                RaiseOnProgress(totalRequestCount++, filteredList.Count, startTime);
                Thread.Sleep(crawlDelay);

                while (activeRequestCounter > 4)
                    Thread.Sleep(500);

                if (totalRequestCount % 10000 == 0)
                {
                    httpClient = NewHttpClientInstance(httpClient, activeRequestCounter);
                    startTime = DateTime.Now;
                    recentRequests = 0;
                }
            }

            while (activeRequestCounter > 0)
                Thread.Sleep(1000);
        }

        int tryCount = 0;
        private HttpClient NewHttpClientInstance(HttpClient httpClient, int activeRequestCounter)
        {
            while (activeRequestCounter > 0 && tryCount++ < 10)
            {
                Console.WriteLine($"Waiting for {activeRequestCounter} to finish before recreating httpClient instance");
                Thread.Sleep(500);
            }

            httpClient.Dispose();
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("User-Agent", Website.UserAgent);
            return httpClient;
        }

        protected abstract string BuildDownloadUrl(string loc);
        protected abstract Func<tUrl, bool> FilterProducts();

        private void RaiseOnStart()
        {
            if (OnStart == null) return;

            OnStart(this, new EventArgs());
        }

        int recentRequests = 0;
        private void RaiseOnProgress(int requestCount, int totalSize, DateTime startTime)
        {
            recentRequests++;

            if (OnProgress == null) return;

            double downloadRate = Math.Max(requestCount / Math.Max(DateTime.Now.Subtract(startTime).TotalSeconds, 1), 1);
            int itemsRemaining = totalSize - requestCount;
            string progressString = $"{requestCount} of {totalSize} { (requestCount / totalSize)}% { downloadRate:0.##}/s ETA:{ DateTime.Now.AddSeconds(itemsRemaining / downloadRate) }        ";
            OnProgress(this, new ProgressEventArgs(progressString));
        }

        private void RaiseOnEnd()
        {
            if (OnEnd == null) return;

            OnEnd(this, new EventArgs());
        }
    }

    public class ProgressEventArgs : EventArgs
    {
        public ProgressEventArgs(string progressString)
        {
            ProgressString = progressString;
        }

        public string ProgressString { get; set; }
    }

    //public byte[] Compress(byte[] inputData)
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

    //public byte[] Decompress(byte[] inputData)
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

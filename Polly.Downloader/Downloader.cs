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
    public class Downloader
    {
        private Thread _mainDownloadThread;
        private Website _website;

        public event EventHandler OnStart;
        public event EventHandler OnEnd;

        public Downloader(Website website)
        {
            _website = website;
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
            DateTime startTime = DateTime.Now;
            Robots robots = new Robots(_website.Domain, _website.UserAgent);
            robots.Load();
            int crawlDelay = robots.GetCrawlDelay();
            var websiteLinksToDownload = robots.GetSitemapLinks();

            var filteredList = websiteLinksToDownload.Where(x => IsProduct(x.loc)).ToList();

            if (crawlDelay != 0)
                crawlDelay = 100;
            else
                crawlDelay = crawlDelay * 1000;

            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("User-Agent", _website.UserAgent);

            int requestCounter = 0;
            int requesttotal = 0;

            foreach (tUrl websiteLink in filteredList)
            {
                requestCounter++;
                Task.Run(async () =>
                {
                    string innerWebsiteLink = websiteLink.loc;
                    var downloadData = new DownloadData()
                    {
                        Url = innerWebsiteLink,
                        WebsiteId = _website.Id
                    };

                    try
                    {
                        downloadData.RawHtml = await httpClient.GetStringAsync(innerWebsiteLink);
                        await DataAccess.AddDownloadDataAsync(downloadData);
                    }
                    finally
                    {
                        requestCounter--;
                    }
                });

                double downloadRate = Math.Max(requesttotal / DateTime.Now.Subtract(startTime).TotalSeconds, 1);
                int itemsRemaining = filteredList.Count - requesttotal++;
                Console.WriteLine($"{requesttotal} of {filteredList.Count} { downloadRate:0.##}/s ETA: { DateTime.Now.AddSeconds(itemsRemaining / downloadRate) }" );
                Console.CursorLeft = 0;
                Console.CursorTop = Console.CursorTop - 1;
                Thread.Sleep(crawlDelay);

                while (requestCounter > 30)
                    Thread.Sleep(1000);
            }

            while (requestCounter > 0)
                Thread.Sleep(1000);
        }

        private bool IsProduct(string url)
        {
            var sections = url.Split('/');
            return !url.Contains("?") && sections.Length == 5 && sections[4].StartsWith("PLID");
        }

        private void RaiseOnStart()
        {
            if (OnStart == null) return;

            OnStart(this, new EventArgs());
        }

        private void RaiseOnEnd()
        {
            if (OnEnd == null) return;

            OnEnd(this, new EventArgs());
        }
    }
}

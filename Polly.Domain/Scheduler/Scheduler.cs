using Polly.Data;
using RobotsSharpParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Polly.Domain
{
    public delegate void ProgressEventHandler(object sender, ProgressEventArgs e);

    public abstract class Scheduler
    {
        private const string UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.102 Safari/537.36";
        private const int BatchSaveCount = 1000;
        private List<DownloadQueue> _saveBatch;
        private DateTime _start;
        private readonly IDownloadQueueRepository _downloadQueueRepository;

        protected abstract string Domain { get; }
        protected abstract int WebsiteId { get; }
        protected abstract string BuildDownloadUrl(string loc);
        protected abstract Func<tUrl, bool> FilterProducts();

        public event ProgressEventHandler OnProgress;

        public Scheduler(IDownloadQueueRepository downloadQueueRepository)
        {
            _downloadQueueRepository = downloadQueueRepository;
            _saveBatch = new List<DownloadQueue>();
        }

        public async Task QueueDownloadLinks()
        {
            Console.WriteLine("Parsing Robots.txt...");
            Robots robots = new Robots(Domain, UserAgent);
            await robots.LoadAsync();
            robots.OnProgress += Robots_OnProgress;
            var sitemaps = await robots.GetSitemapIndexesAsync();

            Console.WriteLine("Saving Robots.txt...");
            int totalRequestCount = 0;
            int totalSoFar = 0;

            foreach (var sitemap in sitemaps)
            {
                var websiteLinksToDownload = await robots.GetUrlsAsync(sitemap);
                var filteredList = websiteLinksToDownload.Where(FilterProducts()).ToList();
                totalSoFar += filteredList.Count;

                _start = DateTime.Now;

                foreach (tUrl websiteLink in filteredList)
                {
                    _saveBatch.Add(new DownloadQueue()
                    {
                        AddedDate = DateTime.Now,
                        DownloadUrl = BuildDownloadUrl(websiteLink.loc),
                        WebsiteId = WebsiteId,
                        Priority = 5,
                    });
                    totalRequestCount++;

                    if (totalRequestCount % BatchSaveCount == 0)
                    {
                        await _downloadQueueRepository.SaveAsync(_saveBatch);
                        _saveBatch.Clear();
                        RaiseOnProgress(totalRequestCount, totalSoFar, _start);
                    }
                }
            }
            await _downloadQueueRepository.SaveAsync(_saveBatch);
        }

        private void Robots_OnProgress(object sender, RobotsSharpParser.ProgressEventArgs e)
        {
            RaiseOnProgress(e.ProgressMessage);
        }

        protected void RaiseOnProgress(int count, int total, DateTime startTime)
        {
            if (OnProgress == null) return;

            double rate = Math.Max(count / Math.Max(DateTime.Now.Subtract(startTime).TotalSeconds, 1), 1);
            int remaining = total - count;
            string progressString = $"{count} of {total} {(count * 1.00 / total * 1.00 * 100):0.####}% { rate:0.##}/s ETA:{ DateTime.Now.AddSeconds(remaining / rate) }        ";

            OnProgress(this, new ProgressEventArgs(progressString));
        }

        protected void RaiseOnProgress(string progressMessage)
        {
            if (OnProgress == null) return;

            OnProgress(this, new ProgressEventArgs(progressMessage));
        }
    }
}

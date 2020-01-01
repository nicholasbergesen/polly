using Polly.Data;
using RobotsSharpParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polly.Domain
{
    public abstract class RobotsBase
    {
        private const string UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.97 Safari/537.36";

        protected abstract string Domain { get; }
        protected abstract int WebsiteId { get; }
        protected abstract string BuildDownloadUrl(string loc);
        protected abstract Func<tUrl, bool> FilterProducts();
        protected int Start = 0;
        private bool _downloadRunnung = false;
        private Task DownloadTask;
        private readonly List<string> _sitemapUrls = new List<string>();
        private readonly object _collectionLock = new object();


        protected async Task<IList<string>> GetSitemapLinks(int batchSize)
        {
            //if (!_downloadRunnung)
            //    DownloadTask = DownloadFromRobots();

            //while(_sitemapUrls.Count < (Start + batchSize) && !DownloadTask.IsCompleted)
            //    await Task.Delay(100);

            await DownloadFromRobots();
            return _sitemapUrls.ToList();
            //lock (_collectionLock)
            //{
            //    return _sitemapUrls.Skip(Start).Take(batchSize).ToList();
            //}
        }

        protected async Task<IEnumerable<DownloadQueueRepositoryItem>> GetNextBatchInternalAsync(int batchSize)
        {
            var returnBatch = await GetSitemapLinks(batchSize);
            Start += returnBatch.Count;
            return returnBatch.Select(x => new DownloadQueueRepositoryItem()
            {
                DownloadLink = x,
                websiteId = WebsiteId
            });
        }

        private async Task DownloadFromRobots()
        {
            _downloadRunnung = true;
            Robots robots = new Robots(Domain, UserAgent)
            {
                IgnoreErrors = true
            };

            await robots.LoadAsync();
            robots.OnProgress += Robots_OnProgress;
            var sitemaps = await robots.GetSitemapIndexesAsync();

            foreach (var sitemap in sitemaps)
            {
                var websiteLinksToDownload = await robots.GetUrlsAsync(sitemap);
                var filteredList = websiteLinksToDownload.Where(FilterProducts());

                foreach (tUrl websiteLink in filteredList)
                {
                    lock (_collectionLock)
                    {
                        _sitemapUrls.Add(BuildDownloadUrl(websiteLink.loc));
                    }
                }
            }
        }

        private void Robots_OnProgress(object sender, RobotsSharpParser.ProgressEventArgs e)
        {
            Console.WriteLine(e.ProgressMessage);
        }
    }
}

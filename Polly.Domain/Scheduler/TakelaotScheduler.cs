using Polly.Data;
using RobotsSharpParser;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Polly.Domain
{
    public class TakelaotScheduler : Scheduler, ITakealotScheduler
    {
        private const string TakealotApi = "https://api.takealot.com/rest/v-1-8-0/product-details";
        private const string Domain = "https://www.takealot.com/";
        private const string UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.102 Safari/537.36";
        private const int WebsiteId = 1;
        private const int BatchSaveCount = 1000;

        private readonly IDownloadQueueRepository _downloadQueueRepository;
        private List<DownloadQueue> _saveBatch;

        public TakelaotScheduler(IDownloadQueueRepository downloadQueueRepository)
        {
            _downloadQueueRepository = downloadQueueRepository;
            _saveBatch = new List<DownloadQueue>();
        }

        public async Task QueueDownloadLinks()
        {
            Robots robots = new Robots(Domain, UserAgent, enableErrorCorrection: true);
            await robots.LoadAsync();
            var websiteLinksToDownload = await robots.GetSitemapLinksAsync();
            var filteredList = websiteLinksToDownload.Where(FilterProducts()).ToList();

            foreach (tUrl websiteLink in filteredList)
            {
                _saveBatch.Add(new DownloadQueue()
                {
                    AddedDate = DateTime.Now,
                    DownloadUrl = BuildDownloadUrl(websiteLink.loc),
                    WebsiteId = WebsiteId,
                    Priority = 5,
                });

                if (_saveBatch.Count == BatchSaveCount)
                {
                    await _downloadQueueRepository.SaveAsync(_saveBatch);
                    _saveBatch.Clear();
                }
            }

            await _downloadQueueRepository.SaveAsync(_saveBatch);
        }

        protected override string BuildDownloadUrl(string loc)
        {
            int lastindex = loc.LastIndexOf('/');
            return string.Concat(TakealotApi, loc.Substring(lastindex, loc.Length - lastindex), "?platform=desktop");
        }

        protected override Func<tUrl, bool> FilterProducts()
        {
            return x => IsProduct(x.loc);
        }

        private bool IsProduct(string url)
        {
            var sections = url.Split('/');
            return !url.Contains("?") && sections.Length == 5 && sections[4].StartsWith("PLID");
        }
    }
}

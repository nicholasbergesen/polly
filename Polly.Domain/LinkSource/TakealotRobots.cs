using Polly.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Polly.Domain
{
    public class TakealotRobots : RobotsBase, ILinkSource
    {
        private const string TakealotApi = "https://api.takealot.com/rest/v-1-11-0/product-details";
        protected override int WebsiteId => 1;
        protected override string Domain => "https://www.takealot.com/";

        public async Task<IEnumerable<DownloadQueueRepositoryItem>> GetNextBatchAsync(int batchSize)
        {
            return await GetNextBatchInternalAsync(batchSize);
        }

        public override string BuildDownloadUrl(string loc)
        {
            int lastindex = loc.LastIndexOf('/');
            return string.Concat(TakealotApi, loc.Substring(lastindex, loc.Length - lastindex), "?platform=desktop");
        }

        public override Func<string, bool> FilterProducts()
        {
            return x => IsProduct(x);
        }

        private bool IsProduct(string url)
        {
            var sections = url?.Split('/');
            return sections != null && !url.Contains("?") && sections.Length == 5 && sections[4].StartsWith("PLID");
        }
    }
}

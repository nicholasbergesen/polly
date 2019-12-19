using Polly.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Polly.Domain
{
    public class LootRobots : RobotsBase, ILinkSource
    {
        protected override int WebsiteId => 2;
        protected override string Domain => "https://www.loot.co.za/";

        public async Task<IEnumerable<DownloadQueueRepositoryItem>> GetNextBatchAsync(int batchSize)
        {
            return await GetNextBatchInternalAsync(batchSize);
        }

        protected override string BuildDownloadUrl(string loc)
        {
            return loc;
        }

        protected override Func<tUrl, bool> FilterProducts()
        {
            return x => { return x.loc.StartsWith("https://www.loot.co.za/product/"); };
        }
    }
}

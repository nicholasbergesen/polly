using Polly.Data;
using System;
using System.Collections.Generic;
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

        public override string BuildDownloadUrl(string loc)
        {
            return loc;
        }

        public override Func<string, bool> FilterProducts()
        {
            return x => { return x.StartsWith("https://www.loot.co.za/product/"); };
        }
    }
}

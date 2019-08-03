using Polly.Data;
using RobotsSharpParser;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Polly.Domain
{
    public class LootScheduler : Scheduler, ILootScheduler
    {
        public LootScheduler(IDownloadQueueRepository downloadQueueRepository)
            : base(downloadQueueRepository)
        {
        }

        protected override string Domain => "https://www.loot.co.za/";

        protected override int WebsiteId => 2;

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

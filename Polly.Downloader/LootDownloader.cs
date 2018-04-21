using Polly.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polly.Downloader
{
    public class LootDownloader : Downloader
    {
        public LootDownloader(Website website)
            :base(website)
        {

        }

        protected override string BuildDownloadUrl(string loc)
        {
            return loc;
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

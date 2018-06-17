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
            if (website.Name != "Loot") throw new ArgumentException("This class only support downloading Loot");
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
            return !url.Contains("?") && sections.Length == 6 && sections[3] == "product";
        }
    }
}

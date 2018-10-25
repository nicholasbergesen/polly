using Polly.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polly.SchedulerConsole
{
    public class TakealotScheduler : Scheduler
    {
        private const string TakealotApi = "https://api.takealot.com/rest/v-1-6-0/productline";

        public TakealotScheduler(Website website)
            : base(website)
        {
            if (website.Name != "Takealot") throw new ArgumentException("This class only supports downloading Takealot");
        }

        protected override string BuildDownloadUrl(string loc)
        {
            return $"{TakealotApi}{loc.Substring(loc.LastIndexOf('/'), loc.Length - loc.LastIndexOf('/'))}";
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

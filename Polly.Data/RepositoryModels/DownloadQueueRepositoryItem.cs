using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polly.Data
{
    public class DownloadQueueRepositoryItem
    {
        public int websiteId;
        public string DownloadLink;

        public override string ToString()
        {
            return $"{websiteId},{DownloadLink}";
        }
    }
}

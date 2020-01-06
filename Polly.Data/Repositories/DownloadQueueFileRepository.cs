using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Polly.Data
{
    public class DownloadQueueFileRepository : IDownloadQueueRepository
    {
        private static object _locker = new object();

        public HashSet<string> GetExistingItems()
        {
            if (File.Exists("downloadLinks.txt"))
                return new HashSet<string>(File.ReadAllLines("downloadLinks.txt"));
            else
                return new HashSet<string>();
        }

        public void SaveBatch(IEnumerable<DownloadQueueRepositoryItem> saveBatch)
        {
            lock (_locker)
            {
                File.AppendAllLines("downloadLinks.txt", saveBatch.Select(x => x.ToString()));
            }
        }
    }
}

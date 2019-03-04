using Polly.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polly.ConsoleNet
{
    public class DownloadQueueFileRepository : IDownloadQueueRepository
    {
        public Task<int> DownloadQueueCountAsync()
        {
            throw new NotImplementedException();
        }

        public Task<DownloadQueue> FetchByIdAsync(long id)
        {
            throw new NotImplementedException();
        }

        public async Task<IReadOnlyList<string>> GetDownloadQueueItems()
        {
            List<string> downloadUrls = new List<string>();
            using (StreamReader sr = new StreamReader("downloadLinks.txt"))
            {
                string line = await sr.ReadLineAsync();
                downloadUrls.Add(line.Split(',')[1]);
            }
            return downloadUrls;
        }

        public Task<IReadOnlyList<long>> GetTopDownloadQueueItems(int size)
        {
            throw new NotImplementedException();
        }

        public async Task RemoveAsync(string downloadQueue)
        {
            using (StreamWriter sr = new StreamWriter("lastUrl.txt"))
                await sr.WriteAsync(downloadQueue);
        }

        public Task RemoveAsync(long downloadQueue)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(DownloadQueue downloadQueue)
        {
            throw new NotImplementedException();
        }

        public Task SaveAsync(IEnumerable<DownloadQueue> saveBatch)
        {
            throw new NotImplementedException();
        }

        Task<IReadOnlyList<long>> IDownloadQueueRepository.GetDownloadQueueItems()
        {
            throw new NotImplementedException();
        }
    }
}

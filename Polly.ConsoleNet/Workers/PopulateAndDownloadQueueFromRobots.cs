using Polly.Data;
using RobotsSharpParser;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Polly.ConsoleNet
{
    public class PopulateAndDownloadQueueFromRobots : IAsyncWorker
    {
        IDownloadQueueRepository _downloadQueueRepository;
        Website _websiteContext;

        public PopulateAndDownloadQueueFromRobots()
        {
        }

        public PopulateAndDownloadQueueFromRobots(IDownloadQueueRepository downloadQueueRepository, Website websitecontext)
        {
            _downloadQueueRepository = downloadQueueRepository;
            _websiteContext = websitecontext;
        }

        public async Task DoWorkAsync(CancellationToken token)
        {
            await Task.Delay(100);
        }
    }
}

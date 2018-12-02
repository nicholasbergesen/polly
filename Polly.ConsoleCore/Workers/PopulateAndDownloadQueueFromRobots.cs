using Polly.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Polly.ConsoleCore
{
    public class PopulateAndDownloadQueueFromRobots : IAsyncWorker
    {
        IDownloadQueueRepository _downloadQueueRepository;

        public PopulateAndDownloadQueueFromRobots(IDownloadQueueRepository downloadQueueRepository)
        {
            _downloadQueueRepository = downloadQueueRepository;
        }

        public async Task DoWorkAsync()
        {
        }
    }
}

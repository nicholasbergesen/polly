using Newtonsoft.Json;
using Polly.Data;
using Polly.Domain;
using System;
using System.Collections.Concurrent;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Polly.ConsoleNet
{
    public class DownloadFromQueue : IAsyncWorker
    {
        private const int TaskCount = 4;

        private int totalQueueCount;
        private DateTime _start;
        private ConcurrentQueue<long> _downloadQueue;

        IDownloadQueueRepository _downloadQueueRepository;
        IDownloader _downloader;
        ITakealotProcessor _takealotProcessor;
        readonly Task[] tasks = new Task[TaskCount];

        public DownloadFromQueue(IDownloadQueueRepository downloadQueueRepository,
            IDownloader downloader,
            ITakealotProcessor takealotProcessor)
        {
            ServicePointManager.DefaultConnectionLimit = 150;
            _downloadQueueRepository = downloadQueueRepository;
            _downloader = downloader;
            _takealotProcessor = takealotProcessor;
        }

        public async Task DoWorkAsync(CancellationToken token)
        {
            totalQueueCount = await _downloadQueueRepository.DownloadQueueCountAsync();
            _downloadQueue = new ConcurrentQueue<long>(await _downloadQueueRepository.GetDownloadQueueItems());

            _start = DateTime.Now;
            for (int i = 0; i < TaskCount; i++)
            {
                tasks[i] = Task.Run(async () =>
                {
                    while (_downloadQueue.TryDequeue(out long id))
                    {
                        var downloadItem = await _downloadQueueRepository.FetchByIdAsync(id);
                        var downloadResult = await _downloader.DownloadAsync(downloadItem.DownloadUrl);
                        if (!string.IsNullOrWhiteSpace(downloadResult))
                            await _takealotProcessor.HandleResultStringAsync(downloadResult);

                        await _downloadQueueRepository.RemoveAsync(downloadItem);
                    }
                }, token);
            }
            var allTasks = Task.WhenAll(tasks);

            while (!allTasks.IsCompleted)
            {
                await Task.Delay(300);
                Console.WriteLine(RaiseOnProgress(totalQueueCount - _downloadQueue.Count, totalQueueCount, _start));
            }
        }

        public static string RaiseOnProgress(int requestCount, int totalSize, DateTime startTime)
        {
            Console.CursorTop = 0;
            double downloadRate = Math.Max(requestCount / Math.Max(DateTime.Now.Subtract(startTime).TotalSeconds, 1), 1);
            int itemsRemaining = totalSize - requestCount;
            return $"{requestCount} of {totalSize} { (requestCount * 1.00 / totalSize * 1.00 * 100):0.####}% { downloadRate:0.##}/s ETA:{ DateTime.Now.AddSeconds(itemsRemaining / downloadRate) }        ";
        }
    }
}

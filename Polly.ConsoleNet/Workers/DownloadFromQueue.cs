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
    public class DownloadFromQueue : AsyncWorkerBase
    {
        private const int TaskCount = 4;

        private int _totalQueueCount;
        private DateTime _start;
        private ConcurrentQueue<long> _downloadQueue;

        private IDownloadQueueRepository _downloadQueueRepository;
        private IDownloader _downloader;
        private ITakealotProcessor _takealotProcessor;
        private readonly Task[] _tasks = new Task[TaskCount];

        public DownloadFromQueue(IDownloadQueueRepository downloadQueueRepository,
            IDownloader downloader,
            ITakealotProcessor takealotProcessor)
        {
            ServicePointManager.DefaultConnectionLimit = 150;
            _downloadQueueRepository = downloadQueueRepository;
            _downloader = downloader;
            _takealotProcessor = takealotProcessor;

            OnProgress += DownloadFromQueue_OnProgress;
            OnStart += DownloadFromQueue_OnStart;
            OnEnd += DownloadFromQueue_OnEnd;
        }

        protected override async Task DoWorkInternalAsync(CancellationToken token)
        {
            RaiseOnStart();

            _downloadQueue = new ConcurrentQueue<long>(await _downloadQueueRepository.GetDownloadQueueItems());
            _totalQueueCount = await _downloadQueueRepository.DownloadQueueCountAsync();

            for (int i = 0; i < TaskCount; i++)
                _tasks[i] = DownloadAndProcess(token);

            var runningTasks = Task.WhenAll(_tasks);
            while (!runningTasks.IsCompleted)
            {
                RaiseOnProgress(_totalQueueCount - _downloadQueue.Count, _totalQueueCount, _start);
                await Task.Delay(1000);
            }
            await runningTasks;

            RaiseOnEnd();
        }

        private async Task DownloadAndProcess(CancellationToken token)
        {
            while (_downloadQueue.TryDequeue(out long id))
            {
                if (token.IsCancellationRequested)
                    return;

                var downloadItem = await _downloadQueueRepository.FetchByIdAsync(id);
                var downloadResult = await _downloader.DownloadAsync(downloadItem.DownloadUrl);
                if (!string.IsNullOrWhiteSpace(downloadResult))
                    await _takealotProcessor.HandleResultStringAsync(downloadResult);

                await _downloadQueueRepository.RemoveAsync(downloadItem);
            }
        }

        private void DownloadFromQueue_OnEnd(object sender, EventArgs e)
        {
            Console.WriteLine("Complete.");
        }

        private void DownloadFromQueue_OnStart(object sender, EventArgs e)
        {
            _start = DateTime.Now;
        }

        private void DownloadFromQueue_OnProgress(object sender, ProgressEventArgs e)
        {
            Console.CursorLeft = 0;
            Console.CursorTop = 0;
            Console.WriteLine(e.ProgressString);
        }

        public override string ToString()
        {
            return "DownloadFromQueue";
        }
    }
}

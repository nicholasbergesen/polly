using Polly.Data;
using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Polly.Domain
{
    public class TakealotFullQueueProcessor
    {
        private const int BatchSize = 10000;
        private const int Threads = 12;

        private CancellationTokenSource _tokenSource;
        private HttpClient _httpClient = new HttpClient();
        private IDownloadQueueRepository _downloadQueueRepository;
        private IProductRepository _ProductRepository;
        private IDownloader _downloader;

        public event ProgressEventHandler OnProgress;
        public delegate void ProgressEventHandler(object sender, ProgressEventArgs e);

        public TakealotFullQueueProcessor(IDownloadQueueRepository downloadQueueRepository, IDownloader downloader)
        {
            _downloadQueueRepository = downloadQueueRepository;
            _downloader = downloader;
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/65.0.3325.181 Safari/537.36");
        }

        public async Task DownloadAsync()
        {
            DateTime startTime = DateTime.Now;
            var downloadQueueIds = new ConcurrentQueue<long>(await _downloadQueueRepository.GetTopDownloadQueueItems(BatchSize));

            Task[] tasks = new Task[Threads];
            for (int i = 0; i < Threads; i++)
                tasks[i] = CreateDownloadTask(downloadQueueIds, _tokenSource.Token);

            var runningTasks = Task.WhenAll(tasks);
            var totalSize = _downloadQueueRepository.DownloadQueueCountAsync();
            while (!runningTasks.IsCompleted)
            {
                //RaiseOnProgress(totalSize - downloadQueueIds.Count, totalSize, startTime);
                await Task.Delay(1000);
            }

            await runningTasks;
        }

        private Task CreateDownloadTask(ConcurrentQueue<long> downloadQueueIds, CancellationToken token)
        {
            return Task.Run(() =>
            {
                //var yesterday = DateTime.Today.Subtract(TimeSpan.FromDays(1));

                //while (downloadQueueIds.TryDequeue(out long downloadQueueId) && !token.IsCancellationRequested)
                //{
                //    var downloadQueueItem = await _downloadQueueRepository.FetchByIdAsync(downloadQueueId);
                //    var productUniqueId = GetUniqueIdFromProductUrl(downloadQueueItem.DownloadUrl);

                //    if (_ProductRepository.TryFetchByUniqueId(productUniqueId, out Product product))
                //    {
                //        bool shouldUpdate = product == null || product.LastChecked < yesterday;
                //        if (shouldUpdate)
                //        {
                //            await Task.Delay(1000);
                //            var json = await DownloadInternal(downloadQueueItem.DownloadUrl);
                //            //if(!string.IsNullOrWhiteSpace(json))
                //            //    await SaveProductFromJson(html);

                //            //await DataAccess.DeleteAsync(nextQueueId);
                //        }
                //    }

                //}
            }, token);
        }

        private string GetUniqueIdFromProductUrl(string downloadUrl)
        {
            int slashPos = downloadUrl.LastIndexOf('/');
            int lastMark = downloadUrl.LastIndexOf('?');
            return downloadUrl.Substring(slashPos + 1, lastMark - slashPos - 1);
        }

        private async Task<string> DownloadInternal(string downloadUrl)
        {
            var response = await _httpClient.GetAsync(downloadUrl);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsStringAsync();
            else if (response.StatusCode == HttpStatusCode.Forbidden)
                throw new HttpListenerException(403, "Blocked by takealot");
            else
                return null;
        }

        private void RaiseOnProgress(string progressMessage)
        {
            if (OnProgress == null)
                return;
            OnProgress(this, new ProgressEventArgs(progressMessage));
        }

        public class ProgressEventArgs : EventArgs
        {
            public string ProgressMessage;

            public ProgressEventArgs(string progressMessage)
            {
                ProgressMessage = progressMessage;
            }
        }
    }
}

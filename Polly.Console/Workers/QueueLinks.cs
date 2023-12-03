using Polly.Data;
using Polly.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Polly.Console
{
    public class QueueLinks : SimpleWorker
    {
        readonly IDownloadQueueRepository _repository;
        readonly IEnumerable<ILinkSource> _linkSources;
        HashSet<string> _inQueue;
        const int BatchSize = 10000;
        public QueueLinks(IEnumerable<ILinkSource> sources,
             IDownloadQueueRepository repository)
            : base()
        {
            _linkSources = sources;
            _repository = repository;
        }

        protected override async Task DoWorkInternalAsync(CancellationToken token)
        {
            System.Console.WriteLine("Loading existing queue items...");
            _inQueue = _repository.GetExistingItems();
            System.Console.WriteLine("Loading existing queue items complete.");

            int total = 0;
            foreach (var linkSource in _linkSources)
            {
                System.Console.WriteLine($"Getting links from {linkSource}...");
                var nextBatch = await linkSource.GetNextBatchAsync(BatchSize);
                while (nextBatch.Any())
                {
                    nextBatch = nextBatch.Where(x => !_inQueue.Contains(x.ToString()));
                    var count = nextBatch.Count();
                    total += count;
                    _repository.SaveBatch(nextBatch);
                    System.Console.WriteLine($"Total items:{total}, added {count}.");

                    if (token.IsCancellationRequested)
                        break;

                    if (linkSource is RobotsBase)//doesn't batch, always loads all items
                        break;

                    nextBatch = await linkSource.GetNextBatchAsync(BatchSize);
                }
                System.Console.WriteLine($"Getting links from {linkSource} complete.");

                if (token.IsCancellationRequested)
                    break;
            }
        }

        public override string ToString()
        {
            return "QueueLinks";
        }
    }
}

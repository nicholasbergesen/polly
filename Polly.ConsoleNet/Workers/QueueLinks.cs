﻿using Polly.Data;
using Polly.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Polly.ConsoleNet
{
    public class QueueLinks : SimpleWorker
    {
        IDownloadQueueRepository _repository;
        IEnumerable<ILinkSource> _linkSources;
        HashSet<string> _inQueue;

        public QueueLinks(IEnumerable<ILinkSource> sources,
             IDownloadQueueRepository repository)
            :base()
        {
            _linkSources = sources;
            _repository = repository;
        }

        protected override async Task DoWorkInternalAsync(CancellationToken token)
        {
            Console.WriteLine("Loading existing queue items...");
            _inQueue = _repository.GetExistingItems();
            Console.WriteLine("Loading existing queue items complete.");
            //if (_inQueue.Count > 100000)
            //{
            //    Console.WriteLine($"The queue has {_inQueue.Count}, please clear these items before adding more");
            //    return Task.FromResult(0);
            //}
            //else
            //{
            //    Console.WriteLine();
            //}

            int total = 0;
            foreach (var linkSource in _linkSources)
            {
                Console.WriteLine($"Getting links from {linkSource}...");
                var nextBatch = await linkSource.GetNextBatchAsync(500);
                while (nextBatch.Any())
                {
                    nextBatch = nextBatch.Where(x => !_inQueue.Contains(x.ToString()));
                    var count = nextBatch.Count();
                    total += count;
                    _repository.SaveBatch(nextBatch);
                    Console.WriteLine($"Total items:{total}, added {count}.");

                    if (token.IsCancellationRequested)
                        break;

                    nextBatch = await linkSource.GetNextBatchAsync(500);
                }
                Console.WriteLine($"Getting links from {linkSource} complete.");

                if (token.IsCancellationRequested)
                    break;
            }
        }
    }
}
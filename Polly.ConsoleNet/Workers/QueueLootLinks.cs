using Polly.Domain;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Polly.ConsoleNet
{
    public class QueueLootLinks : AsyncWorkerBase
    {
        ILootScheduler _lootScheduler;

        public QueueLootLinks(ILootScheduler lootScheduler)
        {
            _lootScheduler = lootScheduler;
            _lootScheduler.OnProgress += _lootScheduler_OnProgress; ;
            OnProgress += QueueLootLinks_OnProgress1; ;
        }

        private void QueueLootLinks_OnProgress1(object sender, ProgressEventArgs e)
        {
            RaiseOnProgress($"{e.ProgressString}");
        }

        private void _lootScheduler_OnProgress(object sender, Domain.ProgressEventArgs e)
        {
            Console.CursorTop = 0;
            Console.CursorLeft = 0;
            Console.WriteLine(e.ProgressString);
        }

        protected override async Task DoWorkInternalAsync(CancellationToken token)
        {
            Console.Clear();
            await _lootScheduler.QueueDownloadLinks();
        }

        public override string ToString()
        {
            return "QueueLootLinks";
        }
    }
}

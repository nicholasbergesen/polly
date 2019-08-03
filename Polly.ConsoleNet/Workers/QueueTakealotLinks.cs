using Polly.Domain;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Polly.ConsoleNet
{
    public class QueueTakealotLinks : AsyncWorkerBase
    {
        ITakealotScheduler _takealotScheduler;

        public QueueTakealotLinks(ITakealotScheduler takealotScheduler)
        {
            _takealotScheduler = takealotScheduler;
            _takealotScheduler.OnProgress += _takealotScheduler_OnProgress;
            OnProgress += QueueTakealotLinks_OnProgress;
            OnStart += QueueTakealotLinks_OnStart;
            OnEnd += QueueTakealotLinks_OnEnd;
        }

        private void _takealotScheduler_OnProgress(object sender, Domain.ProgressEventArgs e)
        {
            Console.CursorTop = 0;
            Console.CursorLeft = 0;
            Console.WriteLine(e.ProgressString);
        }

        private void QueueTakealotLinks_OnEnd(object sender, EventArgs e)
        {
        }

        private void QueueTakealotLinks_OnStart(object sender, EventArgs e)
        {
        }

        private void QueueTakealotLinks_OnProgress(object sender, ProgressEventArgs e)
        {
            RaiseOnProgress($"{e.ProgressString}");
        }

        protected override async Task DoWorkInternalAsync(CancellationToken token)
        {
            Console.Clear();
            await _takealotScheduler.QueueDownloadLinks();
        }

        public override string ToString()
        {
            return "QueueTakealotLinks";
        }
    }
}

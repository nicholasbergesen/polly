using Polly.Data;
using Polly.Domain;
using RobotsSharpParser;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Polly.ConsoleNet
{
    public class QueueTakealotLinks : AsyncWorkerBase
    {
        ITakealotScheduler _takealotScheduler;
        Website _websiteContext;

        public QueueTakealotLinks(ITakealotScheduler takealotScheduler)
        {
            _takealotScheduler = takealotScheduler;

            OnProgress += QueueTakealotLinks_OnProgress; ;
            OnStart += QueueTakealotLinks_OnStart; ;
            OnEnd += QueueTakealotLinks_OnEnd; ;
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
            await _takealotScheduler.QueueDownloadLinks();
        }

        public override string ToString()
        {
            return "QueueTakealotLinks";
        }
    }
}

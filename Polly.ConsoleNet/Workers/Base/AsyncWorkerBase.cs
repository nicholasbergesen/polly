using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Polly.ConsoleNet
{
    public abstract class AsyncWorkerBase : IAsyncWorker
    {
        protected abstract Task DoWorkInternalAsync(CancellationToken token);
        public override abstract string ToString();

        public async Task DoWorkAsync(CancellationToken token)
        {
            RaiseOnStart();
            await DoWorkInternalAsync(token);
            RaiseOnEnd();
        }

        public event EventHandler OnStart;
        public event EventHandler OnEnd;
        public event ProgressEventHandler OnProgress;

        protected DateTime WorkerStartTime { get; private set; }
        protected DateTime WorkerEndTime { get; private set; }

        public delegate void ProgressEventHandler(object sender, ProgressEventArgs e);

        protected void RaiseOnStart()
        {
            WorkerStartTime = DateTime.Now;

            if (OnStart == null) return;

            OnStart(this, new EventArgs());
        }

        protected void RaiseOnEnd()
        {
            WorkerEndTime = DateTime.Now;

            if (OnEnd == null) return;

            OnEnd(this, new EventArgs());
        }

        protected void RaiseOnProgress(int count, int total, DateTime startTime)
        {
            if (OnProgress == null) return;

            double rate = Math.Max(count / Math.Max(DateTime.Now.Subtract(startTime).TotalSeconds, 1), 1);
            int remaining = total - count;
            string progressString = $"{count} of {total} {(count * 1.00 / total * 1.00 * 100):0.####}% { rate:0.##}/s ETA:{ DateTime.Now.AddSeconds(remaining / rate) }        ";

            OnProgress(this, new ProgressEventArgs(progressString));
        }

        protected void RaiseOnProgress(string progressMessage)
        {
            if (OnProgress == null) return;

            OnProgress(this, new ProgressEventArgs(progressMessage));
        }
    }

    public class ProgressEventArgs : EventArgs
    {
        public ProgressEventArgs(string progressString)
        {
            ProgressString = progressString;
        }

        public string ProgressString { get; set; }
    }
}

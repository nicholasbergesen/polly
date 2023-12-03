using System;
using System.Threading;
using System.Threading.Tasks;

namespace Polly.Console
{
    public class SimpleWorker : AsyncWorkerBase
    {
        public SimpleWorker()
        {
            OnProgress += SimpleWorker_OnProgress;
            OnStart += SimpleWorker_OnStart;
            OnEnd += SimpleWorker_OnEnd;
        }

        private void SimpleWorker_OnStart(object sender, System.EventArgs e)
        {
            System.Console.Clear();
        }

        private void SimpleWorker_OnEnd(object sender, EventArgs e)
        {
            System.Console.WriteLine($"{ToString()} completed in {WorkerEndTime.Subtract(WorkerStartTime).ToString()}");
        }

        private void SimpleWorker_OnProgress(object sender, ProgressEventArgs e)
        {
            System.Console.CursorTop = 0;
            System.Console.CursorLeft = 0;
            System.Console.WriteLine(e.ProgressString);
        }

        public override string ToString()
        {
            throw new NotImplementedException();
        }

        protected override Task DoWorkInternalAsync(CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}

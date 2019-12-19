using System;
using System.Threading;
using System.Threading.Tasks;

namespace Polly.ConsoleNet
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
            Console.Clear();
        }

        private void SimpleWorker_OnEnd(object sender, EventArgs e)
        {
            Console.WriteLine($"{ToString()} completed in {WorkerEndTime.Subtract(WorkerStartTime).ToString()}");
        }

        private void SimpleWorker_OnProgress(object sender, ProgressEventArgs e)
        {
            Console.CursorTop = 0;
            Console.CursorLeft = 0;
            Console.WriteLine(e.ProgressString);
        }

        public override string ToString()
        {
            throw new System.NotImplementedException();
        }

        protected override Task DoWorkInternalAsync(CancellationToken token)
        {
            throw new System.NotImplementedException();
        }
    }
}

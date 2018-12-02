using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Polly.ConsoleCore
{
    public interface IAsyncWorker
    {
        Task DoWorkAsync();

        event EventHandler OnStart;
        event EventHandler OnEnd;
        void ProgressEventHandler(object sender, ProgressEventArgs e);
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

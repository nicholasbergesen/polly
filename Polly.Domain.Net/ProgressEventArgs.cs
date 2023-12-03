using System;

namespace Polly.Domain
{
    public class ProgressEventArgs : EventArgs
    {
        public ProgressEventArgs(string progressString)
        {
            ProgressString = progressString;
        }

        public string ProgressString { get; set; }
    }
}

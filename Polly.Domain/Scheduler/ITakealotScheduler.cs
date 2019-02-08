using System.Threading.Tasks;
using static Polly.Domain.TakelaotScheduler;

namespace Polly.Domain
{
    public interface ITakealotScheduler
    {
        Task QueueDownloadLinks();
        event ProgressEventHandler OnProgress;
    }
}
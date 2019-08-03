using System.Threading.Tasks;

namespace Polly.Domain
{
    public interface IScheduler
    {
        Task QueueDownloadLinks();
        event ProgressEventHandler OnProgress;
    }

    public interface ITakealotScheduler : IScheduler
    {
        
    }

    public interface ILootScheduler : IScheduler
    {

    }
}

using System.Threading.Tasks;

namespace Polly.Domain
{
    public interface ITakealotScheduler
    {
        Task QueueDownloadLinks();
    }
}
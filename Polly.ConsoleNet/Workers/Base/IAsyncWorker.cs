using System.Threading;
using System.Threading.Tasks;

namespace Polly.ConsoleNet
{
    public interface IAsyncWorker
    {
        Task DoWorkAsync(CancellationToken token);
    }
}

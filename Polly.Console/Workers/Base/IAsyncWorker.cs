using System.Threading;
using System.Threading.Tasks;

namespace Polly.Console
{
    public interface IAsyncWorker
    {
        Task DoWorkAsync(CancellationToken token);
    }
}

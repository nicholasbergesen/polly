using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Polly.Data
{
    public interface IDownloadQueueRepository
    {
        Task<IReadOnlyList<long>> GetTopDownloadQueueItems(int size);
        Task<IReadOnlyList<long>> GetDownloadQueueItems();
        Task<int> DownloadQueueCountAsync();
        Task<DownloadQueue> FetchByIdAsync(long id);
        Task RemoveAsync(DownloadQueue downloadQueue);
    }
}

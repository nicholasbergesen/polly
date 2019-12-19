using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Polly.Data
{
    public interface IDownloadQueueRepository
    {
        void SaveBatch(IEnumerable<DownloadQueueRepositoryItem> saveBatch);
        HashSet<string> GetExistingItems();
    }
}

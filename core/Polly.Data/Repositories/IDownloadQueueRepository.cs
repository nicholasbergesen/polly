using System.Collections.Generic;

namespace Polly.Data
{
    public interface IDownloadQueueRepository
    {
        void SaveBatch(IEnumerable<DownloadQueueRepositoryItem> saveBatch);
        HashSet<string> GetExistingItems();
    }
}

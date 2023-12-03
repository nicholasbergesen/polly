using Polly.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Polly.Domain
{
    public interface ILinkSource
    {
        Task<IEnumerable<DownloadQueueRepositoryItem>> GetNextBatchAsync(int batchSize); 
    }
}

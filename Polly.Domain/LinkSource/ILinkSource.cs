using Polly.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polly.Domain
{
    public interface ILinkSource
    {
        Task<IEnumerable<DownloadQueueRepositoryItem>> GetNextBatchAsync(int batchSize);
    }
}

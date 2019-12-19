using Polly.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polly.Domain
{
    public class RefreshDatabase : ILinkSource
    {
        private int _start = 0;

        public async Task<IEnumerable<DownloadQueueRepositoryItem>> GetNextBatchAsync(int batchSize)
        {
            var returnBatch = await DataAccess.GetRefreshItems(_start, batchSize);
            _start += batchSize;
            return returnBatch;
        }
    }
}

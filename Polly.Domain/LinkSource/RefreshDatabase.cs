using Polly.Data;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Polly.Domain
{
    public class RefreshDatabase : ILinkSource
    {
        private int _start = 0;
        private readonly IDataAccess _dataAccess;
        public RefreshDatabase(IDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }
        public async Task<IEnumerable<DownloadQueueRepositoryItem>> GetNextBatchAsync(int batchSize)
        {
            var returnBatch = await _dataAccess.GetRefreshItems(_start, batchSize);
            _start += batchSize;
            return returnBatch;
        }
    }
}

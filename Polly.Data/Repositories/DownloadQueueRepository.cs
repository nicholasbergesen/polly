using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Polly.Data
{
    public class DownloadQueueRepository : IDownloadQueueRepository
    {
        public async Task<DownloadQueue> FetchByIdAsync(long id)
        {
            using (PollyDbContext context = new PollyDbContext())
            {
                return await (from downloadQueue in context.DownloadQueue
                        where downloadQueue.Id == id
                        select downloadQueue)
                        .FirstOrDefaultAsync();
            }
        }

        public async Task<IReadOnlyList<long>> GetTopDownloadQueueItems(int size)
        {
            using (PollyDbContext context = new PollyDbContext())
            {
                return await (from downloadQueue in context.DownloadQueue
                        orderby downloadQueue.Priority
                        select downloadQueue.Id)
                        .Take(size)
                        .ToListAsync();
            }
        }

        public async Task<IReadOnlyList<long>> GetDownloadQueueItems()
        {
            using (PollyDbContext context = new PollyDbContext())
            {
                return await (from downloadQueue in context.DownloadQueue
                              orderby downloadQueue.Priority
                              select downloadQueue.Id)
                        .ToListAsync();
            }
        }

        public virtual async Task SaveAsync(DownloadQueue domainObject)
        {
            using (PollyDbContext context = new PollyDbContext())
            {
                if (domainObject.Id == default(long))
                    context.Entry(domainObject).State = EntityState.Added;
                else
                    context.Entry(domainObject).State = EntityState.Modified;

                await context.SaveChangesAsync();
            }
        }

        public async Task SaveAsync(IEnumerable<DownloadQueue> saveBatch)
        {
            using (PollyDbContext context = new PollyDbContext())
            {
                context.DownloadQueue.AddRange(saveBatch);
                await context.SaveChangesAsync();
            }
        }

        public async Task<int> DownloadQueueCountAsync()
        {
            using (PollyDbContext context = new PollyDbContext())
            {
                return await context.DownloadQueue.CountAsync();
            }
        }

        public async Task RemoveAsync(DownloadQueue downloadQueue)
        {
            using (PollyDbContext context = new PollyDbContext())
            {
                context.Entry(downloadQueue).State = EntityState.Deleted;
                await context.SaveChangesAsync();
            }
        }

        public Task RemoveAsync(string downloadQueue)
        {
            throw new System.NotImplementedException();
        }

        public Task RemoveAsync(long downloadQueue)
        {
            throw new System.NotImplementedException();
        }
    }
}

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Polly.Data
{
    public class DataAccess
    {
        public static IEnumerable<Website> GetWebsites()
        {
            using (PollyDbContext context = new PollyDbContext())
            {
                return context.Website.ToList();
            }
        }

        public static Website GetWebsiteById(int id)
        {
            using (PollyDbContext context = new PollyDbContext())
            {
                return context.Website.FirstOrDefault(x => x.Id == id);
            }
        }

        public static void AddToDownloadQueue(string url, int websiteId, int priority)
        {
            using (PollyDbContext context = new PollyDbContext())
            {
                context.DownloadQueue.Add(new DownloadQueue()
                {
                    AddedDate = DateTime.Now,
                    WebsiteId = websiteId,
                    DownloadUrl = url,
                    Priority = priority,
                    UrlHash = url.GetHashCode(),
                });
                context.SaveChanges();
            }
        }

        public static void SaveAsync(Product product)
        {
            using (PollyDbContext context = new PollyDbContext())
            {
                if (product.Id == default(long))
                    context.Product.Add(product);
                else
                    context.Entry(product).State = EntityState.Modified;

                context.SaveChanges();
            }
        }

        public static Product FetchProductOrDefault(string uniqueIdentifier)
        {
            using (PollyDbContext context = new PollyDbContext())
            {
                int uniqueHash = uniqueIdentifier.GetHashCode();
                return context.Product
                    .Include(x => x.PriceHistory)
                    .FirstOrDefault(x => x.UniqueIdentifierHash == uniqueHash)
                    ?? new Product()
                    {
                        UniqueIdentifier = uniqueIdentifier,
                        UniqueIdentifierHash = uniqueHash
                    };
            }
        }

        public static async Task<List<DownloadQueue>> GetDownloadQueueBatchAsync(long websiteId, int batchSize, int skip = 0)
        {
            using (PollyDbContext context = new PollyDbContext())
            {
                return await context.DownloadQueue
                    .Where(x => x.WebsiteId == websiteId)
                    .OrderBy(x => x.Priority)
                    .Skip(skip)
                    .Take(batchSize)
                    .ToListAsync();
            }
        }

        public static async Task<DownloadQueue> GetNextDownloadQueueItemAsync(long websiteId)
        {
            using (PollyDbContext context = new PollyDbContext())
            {
                return await (from downloadQueue in context.DownloadQueue
                              where downloadQueue.WebsiteId == websiteId
                              orderby downloadQueue.Id
                              select downloadQueue)
                             .FirstOrDefaultAsync();
            }
        }

        public static DownloadQueue GetDownloadQueueItemByIdAsync(long Id)
        {
            using (PollyDbContext context = new PollyDbContext())
            {
                return (from downloadQueue in context.DownloadQueue
                        where downloadQueue.Id == Id
                        select downloadQueue)
                        .FirstOrDefault();
            }
        }

        public static ConcurrentQueue<long> GetDownloadQueueIdsAsync(long websiteId)
        {
            using (PollyDbContext context = new PollyDbContext())
            {
                return new ConcurrentQueue<long>((from downloadQueue in context.DownloadQueue
                                                  where downloadQueue.WebsiteId == websiteId
                                                  orderby downloadQueue.Priority
                                                  select downloadQueue.Id).Take(100000));
            }
        }

        public static ConcurrentQueue<long> GetDownloadDataIdsAsync(long websiteId)
        {
            using (PollyDbContext context = new PollyDbContext())
            {
                return new ConcurrentQueue<long>((from downloadQueue in context.DownloadData
                                                  where downloadQueue.WebsiteId == websiteId
                                                  select downloadQueue.Id));
            }
        }

        public static int DownloadQueueCountAsync(long websiteId)
        {
            using (PollyDbContext context = new PollyDbContext())
            {
                return context.DownloadQueue
                     .Count(x => x.WebsiteId == websiteId);
            }
        }

        public async static Task SaveAsync(DownloadQueue downloadQueue)
        {
            using (PollyDbContext context = new PollyDbContext())
            {
                if (downloadQueue.Id == default(long))
                    context.DownloadQueue.Add(downloadQueue);
                else
                    context.Entry(downloadQueue).State = System.Data.Entity.EntityState.Modified;

                await context.SaveChangesAsync();
            }
        }

        public static void DeleteAsync(DownloadQueue downloadQueue)
        {
            using (PollyDbContext context = new PollyDbContext())
            {
                context.Configuration.AutoDetectChangesEnabled = false;
                context.DownloadQueue.Attach(downloadQueue);
                context.Entry(downloadQueue).State = EntityState.Deleted;
                context.SaveChanges();
            }
        }

        public static void DeleteAsync(long downloadQueueId)
        {
            using (PollyDbContext context = new PollyDbContext())
            {
                DownloadQueue deleetme = new DownloadQueue() { Id = downloadQueueId };
                context.Entry(deleetme).State = EntityState.Deleted;
                context.SaveChanges();
            }
        }

        public async static Task SaveAsync(List<DownloadQueue> downloadQueue)
        {
            using (PollyDbContext context = new PollyDbContext())
            {
                context.DownloadQueue.AddRange(downloadQueue);
                await context.SaveChangesAsync();
            }
        }

        public async static Task SaveAsync(Website website)
        {
            using (PollyDbContext context = new PollyDbContext())
            {
                if (website.Id == default(long))
                    context.Website.Add(website);
                else
                    context.Entry(website).State = EntityState.Modified;

                await context.SaveChangesAsync();
            }
        }

        public static void SaveAsync(DownloadData downloadData)
        {
            using (PollyDbContext context = new PollyDbContext())
            {
                if (downloadData.Id == default(long))
                    context.DownloadData.Add(downloadData);
                else
                    context.Entry(downloadData).State = EntityState.Modified;

                context.SaveChanges();
            }
        }

        public static void DeleteAsync(DownloadData downloadData)
        {
            using (PollyDbContext context = new PollyDbContext())
            {
                context.Entry(downloadData).State = EntityState.Deleted;
                context.SaveChanges();
            }
        }

        public static int UnprocessedCount()
        {
            using (PollyDbContext context = new PollyDbContext())
            {
                return context.DownloadData.Count();
            }
        }

        public static DownloadData GetNextDownloadData(long id)
        {
            using (PollyDbContext context = new PollyDbContext())
            {
                return context.DownloadData
                    .Include(x => x.Website)
                    .FirstOrDefault(x => x.Id == id);
            }
        }
    }
}

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

        public static async Task SaveAsync(List<PriceHistory> priceHistories)
        {
            using (PollyDbContext context = new PollyDbContext())
            {
                foreach (var priceHistory in priceHistories)
                {
                    if (priceHistory.Id == default(long))
                        context.PriceHistory.Add(priceHistory);
                    else
                        context.Entry(priceHistory).State = EntityState.Modified;
                }

                await context.SaveChangesAsync();
            }
        }

        public static async Task SaveAsync(Product product)
        {
            using (PollyDbContext context = new PollyDbContext())
            {
                if (product.Id == default(long))
                    context.Product.Add(product);
                else
                    context.Entry(product).State = EntityState.Modified;

                await context.SaveChangesAsync();
            }
        }

        public static async Task SaveAsync(PriceHistory priceHistory)
        {
            using (PollyDbContext context = new PollyDbContext())
            {
                if (priceHistory.Id == default(long))
                    context.PriceHistory.Add(priceHistory);
                else
                    context.Entry(priceHistory).State = EntityState.Modified;

                await context.SaveChangesAsync();
            }
        }

        public static Product FetchProductOrDefault(string uniqueIdentifier)
        {
            using (PollyDbContext context = new PollyDbContext())
            {
                return context.Product
                    .Include(x => x.PriceHistory)
                    .FirstOrDefault(x => x.UniqueIdentifier == uniqueIdentifier);
            }
        }

        public static async Task<Product> FetchProductOrDefault(long productId)
        {
            using (PollyDbContext context = new PollyDbContext())
            {
                return await context.Product
                    .Include(x => x.PriceHistory)
                    .FirstAsync (x => x.Id == productId);
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

        public static async Task<string> GetDownloadQueueItemByIdAsync(long Id)
        {
            using (PollyDbContext context = new PollyDbContext())
            {
                return await (from downloadQueue in context.DownloadQueue
                              where downloadQueue.Id == Id
                              select downloadQueue.DownloadUrl)
                        .FirstOrDefaultAsync();
            }
        }

        public static ConcurrentQueue<long> GetDownloadQueueIds(long websiteId)
        {
            using (PollyDbContext context = new PollyDbContext())
            {
                return new ConcurrentQueue<long>(from downloadQueue in context.DownloadQueue
                                                 where downloadQueue.WebsiteId == websiteId
                                                 orderby downloadQueue.Priority
                                                 select downloadQueue.Id);
            }
        }

        public static ConcurrentQueue<long> GetDownloadQueueProductIds()
        {
            var today = DateTime.Today;
            using (PollyDbContext context = new PollyDbContext())
            {
                return new ConcurrentQueue<long>(from product in context.Product
                                                 where product.LastChecked < today
                                                 orderby product.LastChecked
                                                 select product.Id);
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

        public static async Task<PriceHistory> FetchProductLastPrice(long productId)
        {
            using (PollyDbContext context = new PollyDbContext())
            {
                return await (from priceHistory in context.PriceHistory
                              where priceHistory.ProductId == productId
                              orderby priceHistory.Id descending
                              select priceHistory).FirstOrDefaultAsync();
            }
        }

        public async static Task SaveAsync(DownloadQueue downloadQueue)
        {
            using (PollyDbContext context = new PollyDbContext())
            {
                var downloadItemExists = await context.DownloadQueue.FirstOrDefaultAsync(x => x.DownloadUrl == downloadQueue.DownloadUrl);
                if (downloadItemExists != null)
                    return;
                else if (downloadQueue.Id == default(long))
                    context.DownloadQueue.Add(downloadQueue);
                else
                    context.Entry(downloadQueue).State = EntityState.Modified;

                await context.SaveChangesAsync();
            }
        }

        public static async Task DeleteAsync(DownloadQueue downloadQueue)
        {
            using (PollyDbContext context = new PollyDbContext())
            {
                context.Configuration.AutoDetectChangesEnabled = false;
                context.DownloadQueue.Attach(downloadQueue);
                context.Entry(downloadQueue).State = EntityState.Deleted;
                await context.SaveChangesAsync();
            }
        }

        public static async Task DeleteAsync(long downloadQueueId)
        {
            using (PollyDbContext context = new PollyDbContext())
            {
                DownloadQueue deleetme = new DownloadQueue() { Id = downloadQueueId };
                context.Entry(deleetme).State = EntityState.Deleted;
                await context.SaveChangesAsync();
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

        public static async Task SaveAsync(DownloadData downloadData)
        {
            using (PollyDbContext context = new PollyDbContext())
            {
                if (downloadData.Id == default(long))
                    context.DownloadData.Add(downloadData);
                else
                    context.Entry(downloadData).State = EntityState.Modified;

                await context.SaveChangesAsync();
            }
        }

        public static async Task DeleteAsync(DownloadData downloadData)
        {
            using (PollyDbContext context = new PollyDbContext())
            {
                context.Entry(downloadData).State = EntityState.Deleted;
                await context.SaveChangesAsync();
            }
        }

        public static int UnprocessedCount()
        {
            using (PollyDbContext context = new PollyDbContext())
            {
                return context.DownloadData.Count();
            }
        }

        public static async Task<DownloadData> GetNextDownloadDataAsync(long id)
        {
            using (PollyDbContext context = new PollyDbContext())
            {
                return await context.DownloadData.FindAsync(id);
            }
        }
    }
}

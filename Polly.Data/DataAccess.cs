using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
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

        public static IEnumerable<DownloadData> Unprocessed(int batchSize)
        {
            using (PollyDbContext context = new PollyDbContext())
            {
                return context.DownloadData
                    .Where(x => x.ProcessDateTime == null)
                    .Include(x => x.Website)
                    .Include(x => x.Website.DataSourceType)
                    .Take(batchSize).ToList();
            }
        }

        public async static Task SaveAsync(Product product)
        {
            using (PollyDbContext context = new PollyDbContext())
            {
                if (product.Id == default(long))
                    context.Product.Add(product);
                else
                    context.Entry(product).State = System.Data.Entity.EntityState.Modified;

                await context.SaveChangesAsync();
            }
        }

        public static async Task<List<DownloadQueue>> GetDownloadQueueBatchAsync(long websiteId, int batchSize, int skip = 0)
        {
            using (PollyDbContext context = new PollyDbContext())
            {
                return await context.DownloadQueue
                    .Where(x => x.WebsiteId == websiteId)
                    .OrderBy(x => x.Id)
                    .Skip(skip)
                    .Take(batchSize)
                    .ToListAsync();
            }
        }

        public static async Task<int> DownloadQueueCountAsync(long websiteId)
        {
            using (PollyDbContext context = new PollyDbContext())
            {
                return await context.DownloadQueue
                     .CountAsync(x => x.WebsiteId == websiteId);
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
                    context.Entry(website).State = System.Data.Entity.EntityState.Modified;

                await context.SaveChangesAsync();
            }
        }

        public async static Task SaveAsync(DownloadData downloadData)
        {
            using (PollyDbContext context = new PollyDbContext())
            {
                if (downloadData.Id == default(long))
                    context.DownloadData.Add(downloadData);
                else
                    context.Entry(downloadData).State = System.Data.Entity.EntityState.Modified;

                await context.SaveChangesAsync();
            }
        }
    }
}

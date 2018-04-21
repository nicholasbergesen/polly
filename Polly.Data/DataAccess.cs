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
                return context.DownloadData.Where(x => x.ProcessDateTime == null).Include(x => x.Website).Take(batchSize).ToList();
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

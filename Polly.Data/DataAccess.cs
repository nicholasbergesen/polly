using System;
using System.Collections.Generic;
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

        public async static Task SaveWebsite(Website website)
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
    }
}

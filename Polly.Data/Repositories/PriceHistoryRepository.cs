using System.Threading.Tasks;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;

namespace Polly.Data
{
    public class PriceHistoryRepository : IPriceHistoryRepository
    {
        public async Task<PriceHistory> FetchLastPriceForProductId(long productId)
        {
            using (PollyDbContext context = new PollyDbContext())
            {
                return await(from priceHistory in context.PriceHistory
                             where priceHistory.ProductId == productId
                             orderby priceHistory.TimeStamp descending
                             select priceHistory)
                        .FirstOrDefaultAsync();
            }
        }

        public async Task SaveAllAsync(IEnumerable<PriceHistory> priceHistory)
        {
            foreach (PriceHistory priceHistoryItem in priceHistory)
                await SaveAsync(priceHistoryItem);
        }

        public async Task SaveAsync(PriceHistory priceHistory)
        {
            using (PollyDbContext context = new PollyDbContext())
            {
                if (priceHistory.Id == default)
                    context.PriceHistory.Add(priceHistory);
                else
                    context.Entry(priceHistory).State = EntityState.Modified;

                await context.SaveChangesAsync();
            }
        }
    }
}
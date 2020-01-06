using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Polly.Data
{
    public class PriceHistoryRepository : IPriceHistoryRepository
    {
        public async Task<PriceHistory> FetchLastPriceForProductId(long productId)
        {
            using (PollyDbContext context = new PollyDbContext())
            {
                return await (from priceHistory in context.PriceHistory
                              where priceHistory.ProductId == productId
                              orderby priceHistory.TimeStamp descending
                              select priceHistory)
                        .FirstOrDefaultAsync();
            }
        }

        public async Task<PriceHistory> AddPriceIfNewer(long productId, decimal newPrice)
        {
            using (PollyDbContext context = new PollyDbContext())
            {
                return await (from priceHistory in context.PriceHistory
                              where priceHistory.ProductId == productId
                              orderby priceHistory.TimeStamp descending
                              select priceHistory)
                        .FirstOrDefaultAsync();
            }
        }

        public async Task SaveAsync(PriceHistory priceHistory)
        {
            using (PollyDbContext context = new PollyDbContext())
            {
                if (priceHistory.Id == default)
                    context.PriceHistory.Add(priceHistory);
                else
                    context.Entry(priceHistory).State = EntityState.Unchanged; //should never modify price history

                await context.SaveChangesAsync();
            }
        }
    }
}
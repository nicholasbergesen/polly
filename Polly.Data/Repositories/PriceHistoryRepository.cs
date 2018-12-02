using System.Threading.Tasks;
using System.Linq;
using System.Data.Entity;

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

        public async Task SaveAsync(PriceHistory priceHistory)
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
    }
}
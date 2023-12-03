using Microsoft.EntityFrameworkCore;

namespace Polly.Data
{
    public class PriceHistoryRepository : IPriceHistoryRepository
    {
        private readonly IDbContextFactory<PollyDbContext> _contextFactory;
        public PriceHistoryRepository(IDbContextFactory<PollyDbContext> contextFactory)
        {
            _contextFactory = contextFactory ?? throw new ArgumentNullException($"{nameof(contextFactory)} is null");
        }
        public async Task<PriceHistory> FetchLastPriceForProductId(long productId)
        {
            using PollyDbContext context = await _contextFactory.CreateDbContextAsync();
            
            if (context is null) throw new NullReferenceException($"{nameof(context)} is null");

            return await (from priceHistory in context.PriceHistory
                            where priceHistory.ProductId == productId
                            orderby priceHistory.TimeStamp descending
                            select priceHistory)
                    .FirstOrDefaultAsync();
        }

        public async Task<PriceHistory> AddPriceIfNewer(long productId, decimal newPrice)
        {
            using PollyDbContext context = await _contextFactory.CreateDbContextAsync();
            return await (from priceHistory in context.PriceHistory
                            where priceHistory.ProductId == productId
                            orderby priceHistory.TimeStamp descending
                            select priceHistory)
                    .FirstOrDefaultAsync();
        }

        public async Task SaveAsync(PriceHistory priceHistory)
        {
            using PollyDbContext context = await _contextFactory.CreateDbContextAsync();
            if (priceHistory.Id == default)
                context.PriceHistory.Add(priceHistory);
            else
                context.Entry(priceHistory).State = EntityState.Unchanged; //should never modify price history

            await context.SaveChangesAsync();
        }
    }
}
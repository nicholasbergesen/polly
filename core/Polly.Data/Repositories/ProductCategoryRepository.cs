using Microsoft.EntityFrameworkCore;

namespace Polly.Data
{
    public class ProductCategoryRepository : IProductCategoryRepository
    {
        private readonly IDbContextFactory<PollyDbContext> _contextFactory;
        public ProductCategoryRepository(IDbContextFactory<PollyDbContext> contextFactory)
        {
            _contextFactory = contextFactory ?? throw new ArgumentNullException($"{nameof(contextFactory)} is null");
        }
        public async Task SaveAsync(IEnumerable<ProductCategory> productCategory)
        {
            using PollyDbContext context = await _contextFactory.CreateDbContextAsync();
            context.ProductCategory.AddRange(productCategory);
            await context.SaveChangesAsync();
        }

        public async Task<bool> HasCategories(long productId)
        {
            using PollyDbContext context = await _contextFactory.CreateDbContextAsync();
            return await context.ProductCategory.FirstOrDefaultAsync(x => x.ProductId == productId) != null;
        }
    }
}

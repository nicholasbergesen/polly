using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System.Threading.Tasks;

namespace Polly.Data
{
    public class ProductRepository : IProductRepository
    {
        private readonly IDbContextFactory<PollyDbContext> _contextFactory;
        public ProductRepository(IDbContextFactory<PollyDbContext> contextFactory)
        {
            _contextFactory = contextFactory ?? throw new ArgumentNullException($"{nameof(contextFactory)} is null");
        }
        public async Task<Product> FetchFullProductByUniqueIdAsync(string uniqueIdentifier)
        {
            using PollyDbContext context = await _contextFactory.CreateDbContextAsync();

            context.Database.SetCommandTimeout(TimeSpan.FromSeconds(300));
            return await context.Product
                .Include(x => x.PriceHistory)
                .FirstOrDefaultAsync(x => x.UniqueIdentifier == uniqueIdentifier);
        }

        public async Task SaveAsync(Product product)
        {
            using PollyDbContext context = await _contextFactory.CreateDbContextAsync();

            if (product.Id == default)
                context.Product.Add(product);
            else
                context.Entry(product).State = EntityState.Modified;


            await context.SaveChangesAsync();
        }
    }
}

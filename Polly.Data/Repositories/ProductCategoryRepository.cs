using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Polly.Data
{
    public class ProductCategoryRepository : IProductCategoryRepository
    {
        public async Task SaveAsync(IEnumerable<ProductCategory> productCategory)
        {
            using (PollyDbContext context = new PollyDbContext())
            {
                context.ProductCategory.AddRange(productCategory);
                await context.SaveChangesAsync();
            }
        }

        public async Task<bool> HasCategories(long productId)
        {
            using (PollyDbContext context = new PollyDbContext())
            {
                return await context.ProductCategory.FirstOrDefaultAsync(x => x.ProductId == productId) != null;
            }
        }
    }
}

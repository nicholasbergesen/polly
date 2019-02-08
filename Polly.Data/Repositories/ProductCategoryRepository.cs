using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    }
}

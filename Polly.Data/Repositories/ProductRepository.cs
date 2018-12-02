using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Polly.Data
{
    public class ProductRepository : IProductRepository
    {
        public PriceHistory FetchProductLastPrice(long id)
        {
            throw new NotImplementedException();
        }

        public Product FetchProductOrDefault(string prodid)
        {
            throw new NotImplementedException();
        }

        public async Task<Product> FetchByUniqueIdAsync(string uniqueIdentifier)
        {
            using (PollyDbContext context = new PollyDbContext())
            {
                return await context.Product
                    .FirstOrDefaultAsync(x => x.UniqueIdentifier == uniqueIdentifier);
            }
        }

        public async Task SaveAsync(Product product)
        {
            using (PollyDbContext context = new PollyDbContext())
            {
                if (product.Id == default(long))
                    context.Product.Add(product);
                else
                    context.Entry(product).State = EntityState.Modified;

                await context.SaveChangesAsync();
            }
        }
    }
}

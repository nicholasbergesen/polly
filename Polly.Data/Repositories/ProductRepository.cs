﻿using System.Data.Entity;
using System.Threading.Tasks;

namespace Polly.Data
{
    public class ProductRepository : IProductRepository
    {
        public async Task<Product> FetchFullProductByUniqueIdAsync(string uniqueIdentifier)
        {
            using (PollyDbContext context = new PollyDbContext())
            {
                context.Database.CommandTimeout = 300;
                return await context.Product
                    .Include(x => x.PriceHistory)
                    .FirstOrDefaultAsync(x => x.UniqueIdentifier == uniqueIdentifier);
            }
        }

        public async Task SaveAsync(Product product)
        {
            using (PollyDbContext context = new PollyDbContext())
            {
                if (product.Id == default)
                    context.Product.Add(product);
                else
                    context.Entry(product).State = EntityState.Modified;


                await context.SaveChangesAsync();
            }
        }
    }
}

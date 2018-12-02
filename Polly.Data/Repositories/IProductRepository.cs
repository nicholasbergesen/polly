﻿using System.Threading.Tasks;

namespace Polly.Data
{
    public interface IProductRepository
    {
        Product FetchProductOrDefault(string prodid);
        PriceHistory FetchProductLastPrice(long id);
        Task<Product> FetchByUniqueIdAsync(string uniqueIdentifier);
        Task SaveAsync(Product product);
    }
}
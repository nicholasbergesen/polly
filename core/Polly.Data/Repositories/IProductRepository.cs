using System.Threading.Tasks;

namespace Polly.Data
{
    public interface IProductRepository
    {
        Task<Product> FetchFullProductByUniqueIdAsync(string uniqueIdentifier);
        Task SaveAsync(Product product);
    }
}

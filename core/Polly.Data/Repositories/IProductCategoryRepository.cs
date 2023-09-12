using System.Collections.Generic;
using System.Threading.Tasks;

namespace Polly.Data
{
    public interface IProductCategoryRepository
    {
        Task SaveAsync(IEnumerable<ProductCategory> productCategory);
        Task<bool> HasCategories(long productId);
    }
}

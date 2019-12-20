using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polly.Data
{
    public interface IProductCategoryRepository
    {
        Task SaveAsync(IEnumerable<ProductCategory> productCategory);
        Task<bool> HasCategories(long productId);
    }
}

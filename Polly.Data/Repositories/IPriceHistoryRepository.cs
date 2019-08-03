using System.Linq;
using System.Threading.Tasks;

namespace Polly.Data
{
    public interface IPriceHistoryRepository
    {
        Task<PriceHistory> FetchLastPriceForProductId(long productId);
        Task SaveAsync(PriceHistory priceHistory);
    }
}

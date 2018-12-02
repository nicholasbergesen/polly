using Polly.Data;

namespace Polly.Domain
{
    public interface ITakealotMapper
    {
        bool IsValid(TakealotJson takaleotDTO);
        void MapToProduct(TakealotJson sourceObject, Data.Product product);
        void MapToPriceHistory(TakealotJson takealotDTO, PriceHistory priceHistory);
    }
}
using Polly.Data;

namespace Polly.Website.Models
{
    public class DiscountProduct
    {
        public Product Product { get; set; }
        public PriceHistory LastPrice { get; set; }
    }
}
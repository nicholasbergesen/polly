using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Polly.Data
{
    public class PriceHistory
    {
        public PriceHistory()
        {
            if (TimeStamp == default)
                TimeStamp = DateTime.Now;
        }

        public PriceHistory(Product product)
        {
            ProductId = product.Id;

            if (TimeStamp == default)
                TimeStamp = DateTime.Now;
        }

        public PriceHistory(long productId, decimal price, decimal? originalPrice = null)
        {
            ProductId = productId;
            Price = price;
            OriginalPrice = originalPrice;

            if (OriginalPrice.HasValue && OriginalPrice > 0)
            {
                DiscountAmount = OriginalPrice - Price;
                DiscountPercentage = DiscountAmount / OriginalPrice * 100;
            }

            if (TimeStamp == default)
                TimeStamp = DateTime.Now;
        }

        public PriceHistory(PriceHistory previousPriceHistory, decimal price, decimal? originalPrice = null)
        {
            if (previousPriceHistory == null)
                throw new ArgumentNullException(nameof(previousPriceHistory));

            Price = price;
            OriginalPrice = originalPrice;

            if (OriginalPrice.HasValue && OriginalPrice > 0)
            {
                DiscountAmount = OriginalPrice - Price;
                DiscountPercentage = DiscountAmount / OriginalPrice * 100;
            }

            PreviousPriceHistoryId = previousPriceHistory.Id;
            PriceChangeAmount = Price - previousPriceHistory.Price;
            PriceChangePercent = PriceChangeAmount / previousPriceHistory.Price * 100;
            ProductId = previousPriceHistory.ProductId;

            if (TimeStamp == default)
                TimeStamp = DateTime.Now;
        }

        public PriceHistory(long productId, decimal previousPrice, long previousId, decimal price, decimal? originalPrice = null)
        {
            Price = price;
            OriginalPrice = originalPrice;

            if (OriginalPrice.HasValue && OriginalPrice > 0)
            {
                DiscountAmount = OriginalPrice - Price;
                DiscountPercentage = DiscountAmount / OriginalPrice * 100;
            }

            ProductId = productId;
            PreviousPriceHistoryId = previousId;
            PriceChangeAmount = Price - previousPrice;
            PriceChangePercent = PriceChangeAmount / previousPrice * 100;

            if (TimeStamp == default(DateTime))
                TimeStamp = DateTime.Now;
        }

        public long Id { get; set; }

        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [DataType(DataType.Currency)]
        public decimal? OriginalPrice { get; set; }

        [Index("IX_PriceHistory_DiscountAmount_PreviousPriceHistoryId", Order = 1), DataType(DataType.Currency)]
        public decimal? DiscountAmount { get; set; }

        //IX_PriceHistory_DiscountPercentage added as script for "INCLUDE" sql feature
        public decimal? DiscountPercentage { get; set; }

        [DataType(DataType.Currency)]
        public decimal? PriceChangeAmount { get; set; }

        public decimal? PriceChangePercent { get; set; }

        public DateTime TimeStamp { get; private set; }

        [Index("IX_PriceHistory_ProductId")]
        public long? ProductId { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        [Index("IX_PriceHistory_PreviousPriceHistoryId"), Index("IX_PriceHistory_DiscountAmount_PreviousPriceHistoryId", Order = 2)]
        public long? PreviousPriceHistoryId { get; set; }

        [ForeignKey("PreviousPriceHistoryId")]
        public virtual PriceHistory PreviousPriceHistory { get; set; }
    }
}

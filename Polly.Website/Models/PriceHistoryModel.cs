using Polly.Data;
using System;

namespace Polly.Website.Models
{
    public class PriceHistoryModel
    {
        public long Id { get; set; }
        public decimal Price { get; set; }
        public decimal? OriginalPrice { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public decimal? PriceChangeAmount { get; set; }
        public decimal? PriceChangePercent { get; set; }
        public DateTime TimeStamp { get; private set; }

        public PriceHistoryModel(PriceHistory priceHistory)
        {
            Id = priceHistory.Id;
            Price = priceHistory.Price;
            OriginalPrice = priceHistory.OriginalPrice;
            DiscountAmount = priceHistory.DiscountAmount;
            DiscountPercentage = priceHistory.DiscountPercentage;
            PriceChangeAmount = priceHistory.PriceChangeAmount;
            PriceChangePercent = priceHistory.PriceChangePercent;
            TimeStamp = priceHistory.TimeStamp;
        }
    }
}
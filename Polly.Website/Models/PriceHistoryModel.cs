using Polly.Data;
using System;
using System.ComponentModel.DataAnnotations;

namespace Polly.Website.Models
{
    public class PriceHistoryModel
    {
        public long Id { get; set; }

        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [DataType(DataType.Currency)]
        public decimal? OriginalPrice { get; set; }

        [DataType(DataType.Currency)]
        public decimal? DiscountAmount { get; set; }

        [PercentageDataType]
        public decimal? DiscountPercentage { get; set; }

        [DataType(DataType.Currency)]
        public decimal? PriceChangeAmount { get; set; }

        [PercentageDataType]
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
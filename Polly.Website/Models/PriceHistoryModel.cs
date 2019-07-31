using Polly.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Polly.Website.Models
{
    public class PriceHistoriesModel
    {
        public PriceHistoryModel Current { get; }
        public PriceHistoryModel Lowest { get; }
        public PriceHistoryModel Highest { get; }
        public List<PriceHistoryModel> List { get; }

        private PriceHistoriesModel(List<PriceHistoryModel> priceHistoryModels, decimal lowestPrice, decimal highestPrice)
        {
            List = priceHistoryModels;
            Current = priceHistoryModels.Last();
            Lowest = priceHistoryModels.Last(x => x.Price == lowestPrice);
            Highest = priceHistoryModels.Last(x => x.Price == highestPrice);
        }

        public static PriceHistoriesModel Create(ICollection<PriceHistory> priceHistories)
        {
            var records = priceHistories
                .Select(item => new PriceHistoryModel(item))
                .ToList();

            var lowestPrice = records.Min(x => x.Price);
            var highestPrice = records.Max(x => x.Price);
            return new PriceHistoriesModel(records, lowestPrice, highestPrice);
        }
    }

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
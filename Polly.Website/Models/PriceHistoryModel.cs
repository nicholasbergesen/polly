using Polly.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Polly.Website.Models
{
    public class PriceHistoriesModel
    {
        public decimal Lowest { get; }
        public decimal Highest { get; }
        public List<PriceHistoryModel> List { get; }

        private PriceHistoriesModel(List<PriceHistoryModel> priceHistoryModels)
        {
            List = priceHistoryModels;
            Lowest = priceHistoryModels.Min(x => x.Price);
            Highest = priceHistoryModels.Max(x => x.Price);
        }

        public static PriceHistoriesModel Create(ICollection<PriceHistory> priceHistories)
        {
            var records = priceHistories
                .Select(item => new PriceHistoryModel(item))
                .ToList();

            return new PriceHistoriesModel(records);
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
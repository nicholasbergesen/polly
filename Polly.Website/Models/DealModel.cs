using Polly.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Polly.Website.Models
{
    public class DealModel
    {
        public bool IsGood { get; }

        [DataType(DataType.Currency)]
        public decimal LowestPrice { get; }

        [DataType(DataType.Currency)]
        public decimal CurrentPrice { get; }

        [PercentageDataType]
        public decimal ActualDiscount { get; }

        [PercentageDataType]
        public decimal ClaimedDiscount { get; }

        private DealModel(decimal lowestPrice, decimal currentPrice, decimal actualDiscount, decimal claimedDiscount)
        {
            LowestPrice = lowestPrice;
            CurrentPrice = currentPrice;
            ActualDiscount = actualDiscount;
            ClaimedDiscount = claimedDiscount;
            IsGood = lowestPrice == currentPrice || actualDiscount > claimedDiscount;
        }

        public static DealModel Create(PriceHistoriesModel priceHistories)
        {
            var threshold = DateTime.Today.AddDays(-31);
            var currentRecord = priceHistories.List.Last();
            var records = priceHistories.List.Where(item => item.TimeStamp >= threshold && item.Price != currentRecord.Price);
            var actualDiscount = 100 - ((currentRecord.Price / priceHistories.Highest.Price) * 100);
            return new DealModel(priceHistories.Lowest.Price, currentRecord.Price, actualDiscount, currentRecord.DiscountPercentage ?? 0);
        }
    }
}
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
        public const int DealThreshold = 6;    // Months

        public DealType Type { get; }

        [DataType(DataType.Currency)]
        public decimal LowestPrice { get; }

        [DataType(DataType.Currency)]
        public decimal CurrentPrice { get; }

        [DataType(DataType.Currency)]
        public decimal HighestPrice { get; }

        [PercentageDataType]
        public decimal ActualDiscount { get; }

        [PercentageDataType]
        public decimal ClaimedDiscount { get; }


        public string Threshold { get; }

        private DealModel(DealType dealType, decimal lowestPrice, decimal currentPrice, decimal highestPrice,
            decimal actualDiscount, decimal claimedDiscount)
        {
            Type = dealType;
            LowestPrice = lowestPrice;
            CurrentPrice = currentPrice;
            HighestPrice = highestPrice;
            ActualDiscount = actualDiscount;
            ClaimedDiscount = claimedDiscount;
            Threshold = $"{DealThreshold} months";
        }

        public static DealModel Create(PriceHistoriesModel priceHistories)
        {
            var currentPriceRecord = priceHistories.Current;
            var threshold = DateTime.Today.AddMonths(-DealThreshold);

            var recentPrices = priceHistories.List.Where(item => item.TimeStamp >= threshold);
            var recentLow = recentPrices.Min(m => m.Price);
            var recentHigh = recentPrices.Max(m => m.Price);
            var recentAverage = recentPrices.Average(m => m.Price);

            var actualDiscount = 100 - ((currentPriceRecord.Price / recentHigh) * 100);
            var dealType = GetDealType((int)currentPriceRecord.Price, (int)recentLow, (int)recentAverage, (int)recentHigh);

            return new DealModel(dealType, recentLow, currentPriceRecord.Price, recentHigh, actualDiscount, currentPriceRecord.DiscountPercentage ?? 0);
        }

        private static DealType GetDealType(int currentPrice, int recentLow, int recentAverage, int recentHigh)
        {
            if (recentHigh == currentPrice)
            {
                return DealType.Worst;
            }
            if (recentLow == currentPrice)
            {
                return DealType.Best;
            }
            if (currentPrice < recentHigh && currentPrice > recentAverage)
            {
                return DealType.BelowAverage;
            }
            if (currentPrice > recentLow && currentPrice < recentAverage)
            {
                return DealType.AboveAverage;
            }
            if (GetPriceMarginRange(recentHigh).Contains(currentPrice))
            {
                return DealType.Bad;
            }
            if (GetPriceMarginRange(recentAverage).Contains(currentPrice))
            {
                return DealType.Average;
            }
            if (GetPriceMarginRange(recentLow).Contains(currentPrice))
            {
                return DealType.Good;
            }

            return DealType.Unknown;
        }

        public string GetDealTypeClass()
        {
            switch (Type)
            {
                case DealType.Bad:
                case DealType.Worst:
                    return "text-danger";
                case DealType.BelowAverage:
                case DealType.Average:
                case DealType.AboveAverage:
                    return "text-warning";
                case DealType.Good:
                case DealType.Best:
                    return "text-success";
            }

            return string.Empty;
        }

        public string GetDealTypeText()
        {
            return $"{Type.GetDescription().ToUpper()} DEAL";
        }

        private static IEnumerable<int> GetPriceMarginRange(int price)
        {
            const decimal margin = 0.10m;
            var bottom = price - price * margin;
            var top = price + price * margin;
            return Enumerable.Range((int)bottom, (int)top);
        }
    }

    public enum DealType
    {
        Unknown,
        Worst,
        Bad,
        [Description("Below Average")]
        BelowAverage,
        Average,
        [Description("Above Average")]
        AboveAverage,
        Good,
        Best
    }
}
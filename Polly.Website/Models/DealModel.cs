﻿using Polly.Data;
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
            var lowestPrice = priceHistories.Lowest;
            var highestPrice = priceHistories.Highest;
            var currentRecord = priceHistories.List.Last();
            var actualDiscount = 100 - ((currentRecord.Price / highestPrice) * 100);
            return new DealModel(lowestPrice, currentRecord.Price, actualDiscount, currentRecord.DiscountPercentage ?? 0);
        }
    }
}
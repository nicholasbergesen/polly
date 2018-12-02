﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polly.Data
{
    public class PriceHistory
    {
        public PriceHistory()
        {
            if (TimeStamp == default(DateTime))
                TimeStamp = DateTime.Now;
        }

        public PriceHistory(Product product)
        {
            ProductId = product.Id;

            if (TimeStamp == default(DateTime))
                TimeStamp = DateTime.Now;
        }

        public PriceHistory(PriceHistory previousPriceHistory, decimal price, decimal? originalPrice = null)
        {
            Price = price;
            OriginalPrice = originalPrice;

            if (OriginalPrice.HasValue && OriginalPrice > 0)
            {
                DiscountAmount = OriginalPrice - Price;
                DiscountPercentage = DiscountAmount / OriginalPrice * 100;
            }

            if (previousPriceHistory != null)
            {
                PreviousPriceHistoryId = previousPriceHistory.Id;
                PriceChangeAmount = Price - previousPriceHistory.Price;
                PriceChangePercent = PriceChangeAmount / previousPriceHistory.Price * 100;
            }

            if (TimeStamp == default(DateTime))
                TimeStamp = DateTime.Now;
        }

        public long Id { get; set; }

        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [DataType(DataType.Currency)]
        public decimal? OriginalPrice { get; set; }

        [DataType(DataType.Currency)]
        public decimal? DiscountAmount{ get; set; }

        public decimal? DiscountPercentage { get; set; }

        [DataType(DataType.Currency)]
        public decimal? PriceChangeAmount { get; set; }

        public decimal? PriceChangePercent { get; set; }

        public DateTime TimeStamp { get; private set; }

        public long? ProductId { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        public long? PreviousPriceHistoryId { get; set; }

        [ForeignKey("PreviousPriceHistoryId")]
        public virtual PriceHistory PreviousPriceHistory { get; set; }
    }
}
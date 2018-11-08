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

        public PriceHistory(decimal price, decimal? originalPrice = null)
        {
            Price = price;
            OriginalPrice = originalPrice;

            if (TimeStamp == default(DateTime))
                TimeStamp = DateTime.Now;
        }

        public long Id { get; set; }

        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [DataType(DataType.Currency)]
        public decimal? OriginalPrice { get; set; }

        public DateTime TimeStamp { get; private set; }

        public long? ProductId { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
    }
}
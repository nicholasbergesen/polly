using Polly.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Polly.Website.Models
{
    public class DiscountProduct
    {
        public Product Product { get; set; }
        public PriceHistory LastPrice { get; set; }
    }
}
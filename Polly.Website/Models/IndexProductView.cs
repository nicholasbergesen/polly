using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Polly.Website.Models
{
    public class IndexProductView
    {
        [PercentageDataType]
        public decimal DiscountPercentage { get; set; }
        public string SellingPrice { get; set; }
        public string ImageSrc { get; set; }
        public string Title { get; set; }
        public string TakealotLink { get; set; }
        public string PriceBoarLink { get; set; }

        public override string ToString()
        {
            return $"{DiscountPercentage},{SellingPrice},{ImageSrc},{Title},{TakealotLink},{PriceBoarLink}";
        }
    }
}
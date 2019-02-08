using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Polly.Data
{
    public class Product
    {
        public Product()
        {
            PriceHistory = new List<PriceHistory>();
            ProductCategory = new List<ProductCategory>();
        }

        public long Id { get; set; }

        [Index(IsUnique = true), MaxLength(80)]
        public string UniqueIdentifier { get; set; }

        [Index, DataType(DataType.Url), MaxLength(850)]
        public string Url { get; set; }

        public DateTime LastChecked { get; set; }

        [Index, MaxLength(500)]
        public string Title { get; set; }

        [DataType(DataType.Html)]
        public string Description { get; set; }

        [MaxLength(500)]
        public string Breadcrumb { get; set; }

        [MaxLength(500)]
        public string Category { get; set; }

        [MaxLength(500), DataType(DataType.ImageUrl)]
        public string Image { get; set; }

        public virtual ICollection<PriceHistory> PriceHistory { get; set; }

        public virtual ICollection<ProductCategory> ProductCategory { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace Polly.Website.Core.Data.Models
{
    public class Product
    {
        public Product()
        {
            PriceHistory = new List<PriceHistory>();
            ProductCategory = new List<ProductCategory>();
        }

        [Key]
        public long Id { get; set; }

        [Index(name: "IX_Product_UniqueIdentifier", IsUnique = true), MaxLength(80)]
        public string UniqueIdentifier { get; set; }

        [Index("IX_Product_URL"), DataType(DataType.Url), MaxLength(850)]
        public string Url { get; set; }

        [Index("IX_Product_LastChecked")]
        public DateTime LastChecked { get; set; }

        [Index("IX_Product_Title"), MaxLength(500)]
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

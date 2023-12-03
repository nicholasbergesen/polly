using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Polly.Data
{
    [Index(nameof(UniqueIdentifier), "IX_Product_UniqueIdentifier", IsUnique = true)]
    [Index(nameof(Url), "IX_Product_URL", IsUnique = true)]
    [Index(nameof(LastChecked), "IX_Product_LastChecked", IsUnique = true)]
    [Index(nameof(Title), "IX_Product_Title", IsUnique = true)]
    public class Product
    {
        public Product()
        {
            PriceHistory = new List<PriceHistory>();
            ProductCategory = new List<ProductCategory>();
        }

        [Key]
        public long Id { get; set; }

        [MaxLength(80)]
        public string UniqueIdentifier { get; set; }

        [MaxLength(850)]
        public string Url { get; set; }

        public DateTime LastChecked { get; set; }

        [MaxLength(500)]
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

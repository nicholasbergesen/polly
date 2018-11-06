using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polly.Data
{
    public class Product
    {
        public Product()
        {
            PriceHistory = new List<PriceHistory>();
        }

        public long Id { get; set; }

        public int UniqueIdentifierHash { get; set; }

        [MaxLength(80)]
        public string UniqueIdentifier { get; set; }

        [DataType(DataType.Url), MaxLength(2000)]
        public string Url { get; set; }

        public DateTime LastChecked { get; set; }

        [MaxLength(500)]
        public string Title { get; set; }

        public string Description { get; set; }

        [MaxLength(500)]
        public string Breadcrumb { get; set; }

        [MaxLength(500)]
        public string Category { get; set; }

        [MaxLength(500)]
        public string Image { get; set; }

        public virtual ICollection<PriceHistory> PriceHistory { get; set; }
    }
}

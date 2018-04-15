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
        public long Id { get; set; }

        public int UrlHash { get; set; }

        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [MaxLength(150)]
        public string Name { get; set; }

        public string Description { get; set; }

        public long WebsiteId { get; set; }

        [ForeignKey("WebsiteId")]
        public virtual Website Website { get; set; }

        public ICollection<byte[]> Images { get; set; }
    }
}

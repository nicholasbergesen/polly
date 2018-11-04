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

        public int ProductUniqueIdentifier { get; set; }

        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [MaxLength(300)]
        public string Name { get; set; }

        public string Description { get; set; }

        [MaxLength(500)]
        public string Breadcrumb { get; set; }

        [MaxLength(500)]
        public string Category { get; set; }

        public long DownloadDataId { get; set; }

        [ForeignKey("DownloadDataId")]
        public virtual DownloadData DownloadData { get; set; }

        public ICollection<byte[]> Images { get; set; }
    }
}

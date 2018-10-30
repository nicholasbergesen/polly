using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polly.Data
{
    [Table("DownloadQueue")]
    public class DownloadQueue
    {
        public DownloadQueue()
        {
        }

        [Key]
        public long Id { get; set; }

        public DateTime AddedDate { get; set; }

        [DataType(DataType.Url), MaxLength(2000)]
        public string DownloadUrl { get; set; }

        public int Priority { get; set; }

        public long WebsiteId { get; set; }

        [ForeignKey("WebsiteId")]
        public virtual Website Website { get; set; }

        public int UrlHash { get; set; }
    }
}

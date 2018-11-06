using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polly.Data
{
    [Table("DownloadData")]
    public class DownloadData
    {
        public DownloadData()
        {
            TimeStamp = DateTime.Now;
        }

        [Key]
        public long Id { get; set; }

        public DateTime TimeStamp { get; private set; }

        [DataType(DataType.Url)]
        public string Url { get; set; }

        [DataType(DataType.Html)]
        public string RawHtml { get; set; }

        public long WebsiteId { get; set; }

        [ForeignKey("WebsiteId")]
        public virtual Website Website { get; set; }
    }
}

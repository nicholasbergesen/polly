using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        [Index("PriorityIndex", IsUnique = false)]
        public int Priority { get; set; }

        public long WebsiteId { get; set; }

        [ForeignKey("WebsiteId")]
        public virtual Website Website { get; set; }

        public int UrlHash { get; set; }
    }
}

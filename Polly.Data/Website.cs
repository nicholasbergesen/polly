using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Polly.Data
{
    public class Website
    {
        [Key]
        public long Id { get; set; }

        public string Name { get; set; }

        public string Domain { get; set; }

        public string UserAgent { get; set; }

        [Column(TypeName = "varbinary(max)")]
        public byte[] Logo { get; set; }

        public string TitleXPath { get; set; }

        public string PriceXPath { get; set; }

        public string DescriptionXPath { get; set; }
    }
}
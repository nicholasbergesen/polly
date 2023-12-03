using System.ComponentModel.DataAnnotations;

namespace Polly.Data
{
    public enum DataSourceTypeEnum : int
    {
        Html = 0,
        JSON = 1
    }

    public class DataSourceType
    {
        [Key]
        public DataSourceTypeEnum Id { get; set; }
        public string? Description { get; set; }
    }
}

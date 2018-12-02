using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public string Description { get; set; }
    }
}

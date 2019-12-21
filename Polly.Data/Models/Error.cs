using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polly.Data
{
    public class Error
    {
        public Error()
        {
            if (TimeStamp == default(DateTime))
                TimeStamp = DateTime.Now;
        }

        public Error(Exception exception)
        {
            StackTrace = exception.StackTrace;
            Message = exception.Message;

            if (TimeStamp == default(DateTime))
                TimeStamp = DateTime.Now;
        }

        [Key]
        public int Id { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime TimeStamp { get; set; }

        [MaxLength(8000)]
        public string StackTrace { get; set; }
        [MaxLength(8000)]
        public string Message { get; set; }
    }
}

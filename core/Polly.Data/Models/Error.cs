using System;
using System.ComponentModel.DataAnnotations;

namespace Polly.Data
{
    public class Error
    {
        public Error()
        {
            if (TimeStamp == default)
                TimeStamp = DateTime.Now;
        }

        public Error(Exception exception)
        {
            if (exception is AggregateException agg && agg?.InnerExceptions != null)
            {
                StackTrace = agg?.InnerException?.StackTrace;
                Message = agg?.InnerException?.Message;
            }
            else
            {
                StackTrace = exception.StackTrace;
                Message = exception.Message;
            }

            if (TimeStamp == default(DateTime))
                TimeStamp = DateTime.Now;
        }

        [Key]
        public int Id { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime TimeStamp { get; set; }

        [MaxLength(8000)]
        public string? StackTrace { get; set; }
        [MaxLength(8000)]
        public string? Message { get; set; }
    }
}

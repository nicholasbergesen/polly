using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polly.Data
{
    public class Image
    {
        public long Id { get; set; }

        public int ProductId { get; set; }

        public byte[] ImageData { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polly.Domain
{
    public interface ILinkBuilder
    {
        string BuildDownloadUrl(string loc);
        Func<string, bool> FilterProducts();
    }
}

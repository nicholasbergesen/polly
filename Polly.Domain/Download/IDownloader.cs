using System;
using System.Threading.Tasks;

namespace Polly.Domain
{
    public interface IDownloader : IDisposable
    {
        Task<string> DownloadAsync(string downloadUrl);
        Task<string> DownloadStringAsync(string downloadUrl);
    }
}
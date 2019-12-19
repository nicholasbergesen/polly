using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Polly.Domain
{
    public class TakealotDownloader : IDownloader
    {
        private HttpClient _httpClient = new HttpClient();

        public TakealotDownloader()
        {
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.79 Safari/537.36");
        }

        public async Task<string> DownloadAsync(string downloadUrl)
        {
            var response = await _httpClient.GetAsync(downloadUrl);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsStringAsync();
            else if (response.StatusCode == HttpStatusCode.Forbidden)
                throw new HttpListenerException(403, "Blocked by takealot");
            else
                return null;
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}

using System;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Threading.Tasks;

namespace Polly.Domain
{
    public class TakealotDownloader : IDownloader
    {
        private readonly HttpClient _httpClient;

        public TakealotDownloader()
        {
            _httpClient = new HttpClient();
        }

        public async Task<string> DownloadAsync(string downloadUrl)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Get, new Uri(downloadUrl)))
            {
                request.Headers.TryAddWithoutValidation("accept", "application/json, text/javascript, */*; q=0.01");
                request.Headers.TryAddWithoutValidation("accept-encoding", "gzip, deflate, br");
                request.Headers.TryAddWithoutValidation("accept-language", "en-GB,en-US;q=0.9,en;q=0.8");
                request.Headers.TryAddWithoutValidation("cache-control", "no-cache");
                request.Headers.TryAddWithoutValidation("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.88 Safari/537.36");

                using (var response = await _httpClient.SendAsync(request).ConfigureAwait(false))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        await Data.DataAccess.LogError(new Exception(response.ToString()));
                        return response.ToString();
                    }
                    response.EnsureSuccessStatusCode();

                    using (var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
                    using (var decompressedStream = new GZipStream(responseStream, CompressionMode.Decompress))
                    using (var streamReader = new StreamReader(decompressedStream))
                    {
                        return await streamReader.ReadToEndAsync().ConfigureAwait(false);
                    }
                }
            }
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}

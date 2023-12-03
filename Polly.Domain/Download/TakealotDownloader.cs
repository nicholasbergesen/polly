using Polly.Data;
using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Polly.Domain
{
    public class TakealotDownloader : IDownloader
    {
        private readonly HttpClient _client;
        private readonly IDataAccess _dataAccess;
        public TakealotDownloader(IDataAccess dataAccess)
        {
            SocketsHttpHandler handler = new SocketsHttpHandler
            {
                AutomaticDecompression = (DecompressionMethods.GZip | DecompressionMethods.Deflate),
                AllowAutoRedirect = true,
                CookieContainer = new CookieContainer(),
                UseCookies = true,
            };
            _client = new HttpClient(handler, disposeHandler: true);
            _dataAccess = dataAccess;
        }

        public async Task<string> DownloadAsync(string downloadUrl)
        {
            Uri uri = new Uri(downloadUrl);
            _client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/119.0.0.0 Safari/537.36 Edg/119.0.0.0");
            _client.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "*/*");
            _client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip, deflate, br"));
            _client.DefaultRequestHeaders.TryAddWithoutValidation("AcceptLanguage", "en-US,en;q=0.9,en-ZA;q=0.8");
            _client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue
            {
                MaxAge = TimeSpan.Zero
            };
            //_client.DefaultRequestHeaders.Connection.Add("keep-alive");
            _client.DefaultRequestHeaders.Host = uri.Host;
            _client.DefaultRequestHeaders.TryAddWithoutValidation("Origin", uri.Host);
            _client.DefaultRequestHeaders.Pragma.Add(new NameValueHeaderValue("no-cache"));

            using (var request = new HttpRequestMessage(HttpMethod.Get, new Uri(downloadUrl)))
            {
                request.Headers.TryAddWithoutValidation("accept", "application/json, text/javascript, */*; q=0.01");
                request.Headers.TryAddWithoutValidation("accept-encoding", "gzip, deflate, br");
                request.Headers.TryAddWithoutValidation("accept-language", "en-GB,en-US;q=0.9,en;q=0.8");
                request.Headers.TryAddWithoutValidation("cache-control", "no-cache");
                request.Headers.TryAddWithoutValidation("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.88 Safari/537.36");

                using (var response = await _client.SendAsync(request).ConfigureAwait(false))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        await _dataAccess.LogError(new Exception(response.ToString()));
                        return null;
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
            _client.Dispose();
        }
    }
}

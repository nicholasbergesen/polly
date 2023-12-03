using Polly.Data;
using RobotsParser;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Polly.Domain
{
    public abstract class RobotsBase
    {
        protected abstract string Domain { get; }
        protected abstract int WebsiteId { get; }
        public abstract string BuildDownloadUrl(string loc);
        public abstract Func<string, bool> FilterProducts();
        protected int Start = 0;
        private readonly HashSet<string> _sitemapUrls = new HashSet<string>();


        protected async Task<IList<string>> GetSitemapLinks(int batchSize)
        {
            await DownloadFromRobots();
            return _sitemapUrls.ToList();
        }

        protected async Task<IEnumerable<DownloadQueueRepositoryItem>> GetNextBatchInternalAsync(int batchSize)
        {
            var returnBatch = await GetSitemapLinks(batchSize);
            Start += returnBatch.Count;
            return returnBatch.Select(x => new DownloadQueueRepositoryItem()
            {
                DownloadLink = x,
                websiteId = WebsiteId
            });
        }

        private async Task DownloadFromRobots()
        {
            HttpClient _client = new HttpClient();

            HttpClientHandler handler = new HttpClientHandler
            {
                AutomaticDecompression = (DecompressionMethods.GZip | DecompressionMethods.Deflate),
                AllowAutoRedirect = true,
                CookieContainer = new CookieContainer(),
                UseCookies = true,
                SslProtocols = System.Security.Authentication.SslProtocols.Tls12
            };
            _client = new HttpClient(handler, disposeHandler: true);
            _client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36 Edg/91.0.864.70");
            _client.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7");
            _client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
            _client.DefaultRequestHeaders.TryAddWithoutValidation("AcceptLanguage", "en-US,en;q=0.9,en-ZA;q=0.8");
            _client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue
            {
                MaxAge = TimeSpan.Zero
            };
            _client.DefaultRequestHeaders.Connection.Add("keep-alive");
            _client.DefaultRequestHeaders.Host = "www.takealot.com";
            _client.DefaultRequestHeaders.Pragma.Add(new NameValueHeaderValue("no-cache"));
            Robots robots = new(Domain);
            await robots.LoadRobotsFromUrl("https://www.takealot.com/sitemap.xml");

            var response = await _client.GetAsync(robots.Sitemaps.First());
            var sitemaps = await robots.GetSitemapIndexes();

            foreach (var sitemap in sitemaps)
            {
            }
        }
    }
}

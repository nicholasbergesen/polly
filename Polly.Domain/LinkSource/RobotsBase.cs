using Polly.Data;
using RobotsSharpParser;
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
        private const string UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.97 Safari/537.36";

        protected abstract string Domain { get; }
        protected abstract int WebsiteId { get; }
        protected abstract string BuildDownloadUrl(string loc);
        protected abstract Func<Url, bool> FilterProducts();
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
                AllowAutoRedirect = true
            };
            _client = new HttpClient(handler, disposeHandler: true);
            _client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36 Edg/91.0.864.70");
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xhtml+xml"));
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
            _client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));
            _client.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("en-ZA"));
            _client.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("en-GB"));
            _client.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("en-US"));
            _client.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("en"));
            _client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue
            {
                NoCache = true
            };
            _client.DefaultRequestHeaders.Connection.Add("keep-alive");
            _client.DefaultRequestHeaders.Host = "www.takealot.com";
            _client.DefaultRequestHeaders.Pragma.Add(new NameValueHeaderValue("no-cache"));
            Robots robots = new Robots(Domain, UserAgent)
            {
                IgnoreErrors = true
            };

            await robots.LoadAsync();
            var sitemaps = await robots.GetSitemapIndexesAsync();

            foreach (var sitemap in sitemaps)
            {
                var websiteLinksToDownload = await GetUrlsAsync(sitemap, _client);
                var filteredList = websiteLinksToDownload.Where(FilterProducts());

                foreach (Url websiteLink in filteredList)
                {
                    _sitemapUrls.Add(BuildDownloadUrl(websiteLink.loc));
                }

                Console.WriteLine($"{filteredList.Count()} added to {_sitemapUrls.Count}");
            }
        }

        private bool TryDeserializeXMLStream<T>(byte[] bytes, out T xmlValue)
        {
            using (StringReader reader = new StringReader(Encoding.UTF8.GetString(bytes)))
            {
                return TryDeserializeXMLStream(reader, out xmlValue);
            }
        }

        private bool TryDeserializeXMLStream<T>(TextReader reader, out T xmlValue)
        {
            try
            {
                using (XmlReader xmlReader = XmlReader.Create(reader))
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                    xmlValue = (T)xmlSerializer.Deserialize(xmlReader);
                    return xmlValue != null;
                }
            }
            catch
            {
                xmlValue = default(T);
                return false;
            }
        }

        public async Task<IReadOnlyList<Url>> GetUrlsAsync(tSitemap tSitemap, HttpClient _client)
        {
            if (tSitemap == null)
            {
                throw new ArgumentNullException("tSitemap", "sitemap requires a value");
            }
            /*
             
<?xml version="1.0" encoding="UTF-8"?>
<urlset
  xmlns="https://www.sitemaps.org/schemas/sitemap/0.9" xmlns:xhtml="http://www.w3.org/1999/xhtml">
<url>
    <loc>https://www.takealot.com/the-holy-spirit-in-thought-and-experience/PLID69992870</loc>
    <lastmod>2021-07-14T23:08:27+00:00</lastmod>
    <changefreq>monthly</changefreq>
    <priority>0.6</priority>
    <xhtml:link href="android-app://fi.android.takealot/app/takealot.com/product/PLID69992870" rel="alternate" />
   </url>
<url>
    <loc>https://www.takealot.com/boiler-economy/PLID6627381</loc>
    <lastmod>2021-07-14T23:08:27+00:00</lastmod>
    <changefreq>monthly</changefreq>
    <priority>0.6</priority>
    <xhtml:link href="android-app://fi.android.takealot/app/takealot.com/product/PLID6627381" rel="alternate" />
   </url>             */
            HttpResponseMessage httpResponseMessage = await _client.GetAsync(tSitemap.loc);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    await httpResponseMessage.Content.CopyToAsync(stream);
                    if (!TryDecompress(stream, out byte[] bytes))
                    {
                        bytes = stream.ToArray();
                    }

                    if (TryDeserializeXMLStream(bytes, out urlset xmlValue) && xmlValue.url != null)
                    {
                        return xmlValue.url;
                    }

                    string val = Encoding.UTF8.GetString(bytes);

                    return new List<Url>();
                }
            }

            return new List<Url>();
        }

        private bool TryDecompress(Stream stream, out byte[] bytes)
        {
            try
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    stream.Position = 0L;
                    using (GZipStream gZipStream = new GZipStream(stream, CompressionMode.Decompress))
                    {
                        gZipStream.CopyTo(memoryStream);
                        bytes = memoryStream.ToArray();
                    }
                }

                return true;
            }
            catch
            {
                bytes = null;
                return false;
            }
        }
    }

    [Serializable]
    [XmlType(AnonymousType = true, Namespace = "https://www.sitemaps.org/schemas/sitemap/0.9")]
    [XmlRoot(Namespace = "https://www.sitemaps.org/schemas/sitemap/0.9", IsNullable = false)]
    public class urlset
    {
        private XmlElement[] anyField;

        private Url[] urlField;

        [XmlAnyElement]
        public XmlElement[] Any
        {
            get
            {
                return anyField;
            }
            set
            {
                anyField = value;
            }
        }

        [XmlElement("url")]
        public Url[] url
        {
            get
            {
                return urlField;
            }
            set
            {
                urlField = value;
            }
        }
    }

    [Serializable]
    [XmlType(Namespace = "https://www.sitemaps.org/schemas/sitemap/0.9")]
    public class Url
    {
        private string locField;

        private string lastmodField;

        private tChangeFreq changefreqField;

        private bool changefreqFieldSpecified;

        private decimal priorityField;

        private bool priorityFieldSpecified;

        private link linkField;

        private XmlElement[] anyField;

        [XmlElement(DataType = "anyURI")]
        public string loc
        {
            get
            {
                return locField;
            }
            set
            {
                locField = value;
            }
        }

        public string lastmod
        {
            get
            {
                return lastmodField;
            }
            set
            {
                lastmodField = value;
            }
        }

        public tChangeFreq changefreq
        {
            get
            {
                return changefreqField;
            }
            set
            {
                changefreqField = value;
            }
        }

        [XmlIgnore]
        public bool changefreqSpecified
        {
            get
            {
                return changefreqFieldSpecified;
            }
            set
            {
                changefreqFieldSpecified = value;
            }
        }

        public decimal priority
        {
            get
            {
                return priorityField;
            }
            set
            {
                priorityField = value;
            }
        }

        [XmlIgnore]
        public bool prioritySpecified
        {
            get
            {
                return priorityFieldSpecified;
            }
            set
            {
                priorityFieldSpecified = value;
            }
        }

        [XmlElement(Namespace = "http://www.w3.org/1999/xhtml")]
        public link link
        {
            get
            {
                return linkField;
            }
            set
            {
                linkField = value;
            }
        }

        [XmlAnyElement]
        public XmlElement[] Any
        {
            get
            {
                return anyField;
            }
            set
            {
                anyField = value;
            }
        }
    }
}

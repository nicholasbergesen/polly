using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;
using Polly.Data;
using Polly.Domain;

namespace Polly.Website.Controllers
{
    [Authorize]
    [RoutePrefix("api/products")]
    public class ProductsController : ApiController
    {
        private static Dictionary<string, ApiProd> productPrices = new Dictionary<string, ApiProd>();
        private static Dictionary<string, ApiPriceHistory> productPriceHistory = new Dictionary<string, ApiPriceHistory>();

        private readonly IProductRepository _productRepository;
        private readonly IDownloader _downloader;
        private readonly ITakealotMapper _takealotMapper;

        public ProductsController()
        {
            _productRepository = new ProductRepository();
            _downloader = new TakealotDownloader();
            _takealotMapper = new TakelaotMapper(new PriceHistoryRepository(), _productRepository);
        }

        [Route("{productId}/{currentPrice}")]
        public async Task<HttpResponseMessage> GetLastProductPrice(string productId, decimal currentPrice)
        {
            ApiProd returnPrice;
            if (!productPrices.TryGetValue(productId, out returnPrice))
            {
                productPriceHistory.Remove(productId);
                var thirtyoneDays = DateTime.Today.Subtract(TimeSpan.FromDays(31));

                var productdb = await _productRepository.FetchFullProductByUniqueIdAsync(productId);

                if (productdb != null)
                {
                    var lastPrice = productdb.PriceHistory.Last();
                    if(lastPrice.Price != currentPrice)
                    {
                        //WARNING!
                        //this is very heavy, try sending more data from the client to prevent needing to do a web call on the server.
                        var downloadUrl = BuildDownloadUrl(productId);
                        var html = await _downloader.DownloadAsync(downloadUrl);
                        await _takealotMapper.MapAndSaveAsync(html);

                        await Task.Delay(100);//not to spam takealot
                    }

                    var recentPrices = productdb.PriceHistory.Where(x => thirtyoneDays < x.TimeStamp && x.Price != currentPrice);

                    //1000% increase represents a 90% discount, anything more is considered junk data
                    if (!recentPrices.Any())
                        returnPrice = new ApiProd() { Price = 0, Url = "https://www.priceboar.com/Home/Details/" + productdb.Id, Status = Status.NoPrices };
                    else if (recentPrices.Any(x => x.Price < (currentPrice * 10)))
                        returnPrice = new ApiProd()
                        {
                            Price = recentPrices.Where(x => x.Price < (currentPrice * 10)).Max(x => x.Price),
                            Url = "https://www.priceboar.com/Home/Details/" + productdb.Id,
                            Status = Status.HasValidPrice
                        };
                    else
                        returnPrice = new ApiProd()
                        {
                            Price = recentPrices.Max(x => x.Price),
                            Url = "https://www.priceboar.com/Home/Details/" + productdb.Id,
                            Status = Status.HasValidPrice
                        };
                    productPrices.Add(productId, returnPrice);
                }
                else //add product if it doesn't exist
                {
                    var downloadUrl = BuildDownloadUrl(productId);
                    var html = await _downloader.DownloadAsync(downloadUrl);
                    var productInternal = await _takealotMapper.MapAndSaveAsync(html);
                    returnPrice = new ApiProd() { Price = 0, Url = "https://www.priceboar.com/Home/Details/" + productInternal.Id, Status = Status.RequiresUpdate };
                }
            }

            var response = Request.CreateResponse(HttpStatusCode.OK, returnPrice);
            response.Headers.CacheControl = new System.Net.Http.Headers.CacheControlHeaderValue()
            {
                MaxAge = TimeSpan.FromHours(4),
                Public = true
            };

            return response;
        }

        [Route("pricehistory/{productId}")]
        public async Task<ApiPriceHistory> GetPriceHistory(string productId)
        {
            if (productPriceHistory.TryGetValue(productId, out ApiPriceHistory apiPriceHistory))
            {
                return apiPriceHistory;
            }
            else
            {
                productPriceHistory.Remove(productId);
            }

            var productdb = await _productRepository.FetchFullProductByUniqueIdAsync(productId);

            ApiPriceHistory prices = new ApiPriceHistory();

            foreach (var price in productdb.PriceHistory)
            {
                prices.Price.Add(price.Price);
                prices.Date.Add(price.TimeStamp.ToShortDateString());
            }
            prices.Added = DateTime.Now;

            productPriceHistory.Add(productId, prices);

            return prices;
        }

        private static class Status
        {
            public const string NoPrices = "NoPrices";
            public static string HasValidPrice = "HasValidPrice";
            public static string RequiresUpdate = "RequiresUpdate";
        }

        public class ApiProd
        {
            public decimal Price { get; set; }
            public string Url { get; set; }
            public string Status { get; set; }
        }

        public class ApiPriceHistory
        {
            public ApiPriceHistory()
            {
                Price = new List<decimal>();
                Date = new List<string>();
            }
            public ICollection<decimal> Price { get; set; }
            public ICollection<string> Date { get; set; }
            public DateTime Added { get; set; }
        }

        private const string TakealotApi = "https://api.takealot.com/rest/v-1-9-0/product-details";

        protected string BuildDownloadUrl(string productId)
        {
            return string.Concat(TakealotApi, "/", productId, "?platform=desktop");
        }
    }
}
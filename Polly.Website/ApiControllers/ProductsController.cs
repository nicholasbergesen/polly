using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
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
        private readonly ITakealotMapper _takealotMapper;

        public ProductsController()
        {
            _productRepository = new ProductRepository();
            _takealotMapper = new TakelaotMapper(new PriceHistoryRepository(), _productRepository, new CategoryRepository(), new ProductCategoryRepository());
        }

        [HttpGet]
        [Route("{uniqueIdentifier}/{currentPrice:decimal}")]
        public async Task<HttpResponseMessage> CheckProductUpdate(string uniqueIdentifier, decimal currentPrice)
        {
            ApiProd returnPrice;
            if (!productPrices.TryGetValue(uniqueIdentifier, out returnPrice))
            {
                productPriceHistory.Remove(uniqueIdentifier);

                var productdb = await _productRepository.FetchFullProductByUniqueIdAsync(uniqueIdentifier);

                if (productdb != null)
                {
                    var lastPrice = productdb.PriceHistory.Last();
                    if (lastPrice.Price != currentPrice)
                    {
                        productdb.PriceHistory.Add(new PriceHistory(lastPrice, currentPrice) { ProductId = productdb.Id });
                        productdb.LastChecked = DateTime.Now;
                        await _productRepository.SaveAsync(productdb);
                    }

                    var thirtyoneDays = DateTime.Today.Subtract(TimeSpan.FromDays(31));
                    var recentPrices = productdb.PriceHistory.Where(x => thirtyoneDays < x.TimeStamp && x.Price != currentPrice);

                    returnPrice = new ApiProd() { Url = "https://priceboar.com/Home/Details/" + productdb.Id, Status = Status.Complete };
                    if (!recentPrices.Any())
                        returnPrice.Price = lastPrice.Price;
                    else
                        returnPrice.Price = recentPrices.Max(x => x.Price);

                    productPrices.Add(uniqueIdentifier, returnPrice);

                    var response = Request.CreateResponse(HttpStatusCode.OK, returnPrice);
                    response.Headers.CacheControl = new System.Net.Http.Headers.CacheControlHeaderValue()
                    {
                        MaxAge = TimeSpan.FromHours(12),
                        Public = true
                    };
                    return response;
                }
                else//request add from client if product it doesn't exist
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new ApiProd() { Status = Status.AddProduct });
                }
            }
            else
            {
                var response = Request.CreateResponse(HttpStatusCode.OK, returnPrice);
                response.Headers.CacheControl = new System.Net.Http.Headers.CacheControlHeaderValue()
                {
                    MaxAge = TimeSpan.FromHours(12),
                    Public = true
                };
                return response;
            }
        }

        [HttpPost]
        [Route("addproduct")]
        public async Task<HttpResponseMessage> AddProduct([FromBody] TakealotJson takealotJson)
        {
            if (takealotJson == null)
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            var productInternal = await _takealotMapper.MapAndSaveFullAsync(takealotJson);
            var returnPrice = new ApiProd() { Price = 0, Url = "https://priceboar.com/Home/Details/" + productInternal.Id, Status = Status.Complete };

            if (productPrices.ContainsKey(productInternal.UniqueIdentifier))
                productPrices[productInternal.UniqueIdentifier] = returnPrice;

            var response = Request.CreateResponse(HttpStatusCode.Created, returnPrice);
            response.Headers.CacheControl = new System.Net.Http.Headers.CacheControlHeaderValue()
            {
                MaxAge = TimeSpan.FromHours(12),
                Public = true
            };
            return response;
        }

        [HttpGet]
        [Route("pricehistory/{uniqueIdentifier}")]
        public async Task<ApiPriceHistory> GetPriceHistory(string uniqueIdentifier)
        {
            if (productPriceHistory.TryGetValue(uniqueIdentifier, out ApiPriceHistory apiPriceHistory))
                return apiPriceHistory;

            var productdb = await _productRepository.FetchFullProductByUniqueIdAsync(uniqueIdentifier);
            ApiPriceHistory prices = new ApiPriceHistory();
            foreach (var price in productdb.PriceHistory)
            {
                prices.Price.Add(price.Price);
                prices.Date.Add(price.TimeStamp.ToShortDateString());
            }
            prices.Added = DateTime.Now;

            if(!productPriceHistory.ContainsKey(uniqueIdentifier))
                productPriceHistory.Add(uniqueIdentifier, prices);

            return prices;
        }

        [HttpGet]
        [Route("tquery")]
        public async Task<string> TQuery(string downloadString)
        {
            using(var downloader = new TakealotDownloader())
            {
                return await downloader.DownloadStringAsync(downloadString);
            }
        }

        private static class Status
        {
            public const string AddProduct = "AddProduct";
            public static string Complete = "Complete";
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
    }
}
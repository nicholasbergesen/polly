using Newtonsoft.Json;
using Polly.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Polly.DailyUpdater
{
    public class Program
    {
        private const string TakealotApiPromition = "https://api.takealot.com/rest/v-1-8-0/promotions";
        private const string TakealotApiPromitionProduct = "https://api.takealot.com/rest/v-1-8-0/productlines/search?filter=Promotions:{0}&rows={1}&start={2}";
        private const string TakealotProductDetails = "https://api.takealot.com/rest/v-1-7-0/product-details/{0}";

        private static HttpClient httpClient = new HttpClient();

        static void Main(string[] args)
        {
            httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/65.0.3325.181 Safari/537.36");
            MainAsync(args).Wait();
        }

        static async Task MainAsync(string[] args)
        {
            var promotions = await GetPromotions();
            int count = 0;
            int total = 0;
            DateTime startTime = DateTime.Now;
            try
            {
                foreach (var promotion in promotions)
                {
                    int rows = 100;
                    int start = 0;
                    IEnumerable<Productline> promotionProducts;

                    do
                    {
                        promotionProducts = await GetPromotionProducts(promotion.promotion_id, rows, start);
                        start += rows;
                        total += promotionProducts.Count();
                        foreach (var product in promotionProducts)
                        {
                            Console.WriteLine(RaiseOnProgress(count++, total, startTime));

                            decimal price = (decimal)product.selling_price / 100;
                            decimal originalPrice = (decimal)product.old_selling_price / 100;
                            var domainProduct = DataAccess.FetchProductOrDefault(product.id);
                            if (domainProduct != null)
                            {
                                var lastPrice = domainProduct.PriceHistory.Last();
                                if (lastPrice.Price == price)
                                {
                                    continue;
                                }
                                else
                                {
                                    var priceHistory = new PriceHistory(price, originalPrice)
                                    {
                                        ProductId = domainProduct.Id
                                    };
                                    await DataAccess.SaveAsync(priceHistory);
                                }
                            }
                            else
                            {
                                var productDetail = await GetProduct(string.Format(TakealotProductDetails, product.id));
                                if (productDetail == null) continue;
                                domainProduct = new Data.Product()
                                {
                                    UniqueIdentifier = product.id,
                                    Title = product.title,
                                };
                                domainProduct.PriceHistory.Add(new PriceHistory(price, originalPrice));
                                domainProduct.Url = product.uri;
                                domainProduct.LastChecked = DateTime.Now;

                                domainProduct.Breadcrumb = productDetail.breadcrumbs?.items.Select(x => x.name).Aggregate((i, j) => i + ", " + j);
                                domainProduct.Description = productDetail.description?.html;
                                domainProduct.Category = productDetail.data_layer.categoryname?.Select(x => x).Aggregate((i, j) => i + ", " + j);
                                domainProduct.Image = productDetail.gallery.images[0].Replace("{size}", "pdpxl");
                                await DataAccess.SaveAsync(domainProduct);
                            }
                        }
                    }
                    while (promotionProducts.Count() == rows);
                }
            }
            catch(Exception e)
            {

            }
        }

        public static string RaiseOnProgress(int requestCount, int totalSize, DateTime startTime)
        {
            Console.CursorLeft = 0;
            Console.CursorTop = 1;
            double downloadRate = Math.Max(requestCount / Math.Max(DateTime.Now.Subtract(startTime).TotalSeconds, 1), 1);
            int itemsRemaining = totalSize - requestCount;
            return $"{requestCount} of {totalSize} { (requestCount * 1.00 / totalSize * 1.00 * 100):0.####}% { downloadRate:0.##}/s ETA:{ DateTime.Now.AddSeconds(itemsRemaining / downloadRate) }        ";
        }

        public static async Task<IEnumerable<Response>> GetPromotions()
        {
            var responseString = await httpClient.GetStringAsync(TakealotApiPromition);
            return JsonConvert.DeserializeObject<TakealotPromotion>(responseString).response;
        }

        public static async Task<IEnumerable<Productline>> GetPromotionProducts(int promotionId, int rows, int start)
        {
            var responseString = await httpClient.GetStringAsync(string.Format(TakealotApiPromitionProduct, promotionId, rows, start));
            return JsonConvert.DeserializeObject<TakealotPromotionProduct>(responseString).results.productlines;
        }

        public static async Task<Prod.TakealotJson> GetProduct(string downloadUrl)
        {
            string responseString;
            var response = await httpClient.GetAsync(downloadUrl);
            if (response.IsSuccessStatusCode)
            {
                responseString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Prod.TakealotJson>(responseString);
            }
            return null;
        }
    }
}

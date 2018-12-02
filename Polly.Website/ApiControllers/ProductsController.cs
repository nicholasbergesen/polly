﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;
using Polly.Data;
using Polly.Website.Controllers.Json;

namespace Polly.Website.Controllers
{

    [RoutePrefix("api2/products")]
    public class ProductsController : ApiController
    {
        private static Dictionary<string, ApiProd> productPrices = new Dictionary<string, ApiProd>();

        private PollyDbContext db = new PollyDbContext();

        [Route("{productId}/{currentPrice}")]
        public async Task<HttpResponseMessage> GetLastProductPrice(string productId, decimal currentPrice)
        {
            ApiProd returnPrice;
            if (!productPrices.TryGetValue(productId, out returnPrice))
            {
                var twoWeeksAgo = DateTime.Today.Subtract(TimeSpan.FromDays(31));

                var productdb = await (from product in db.Product
                                       where product.UniqueIdentifier == productId
                                       select product)
                                     .Include(x => x.PriceHistory)
                                     .FirstOrDefaultAsync();

                if (productdb != null)
                {
                    var recentPrices = productdb.PriceHistory.Where(x => twoWeeksAgo < x.TimeStamp && x.Price != currentPrice);
                    //1000% increase represents a 90% discount, anything more is considered junk data
                    if (!recentPrices.Any())
                        returnPrice = new ApiProd() { Price = 0, Url = "https://www.pollychron.com/Home/Details/" + productdb.Id };
                    else if (recentPrices.Any(x => x.Price < (currentPrice * 10)))
                        returnPrice = new ApiProd() { Price = recentPrices.Where(x => x.Price < (currentPrice * 10)).Max(x => x.Price), Url = "https://www.pollychron.com/Home/Details/" + productdb.Id };
                    else
                        returnPrice = new ApiProd() { Price = recentPrices.Max(x => x.Price), Url = "https://www.pollychron.com/Home/Details/" + productdb.Id };

                    var lastPrice = productdb.PriceHistory.Last();
                    if(lastPrice.Price != currentPrice)
                    {
                        await Task.Delay(100);
                        var downloadUrl = BuildDownloadUrl(productId);
                        using (HttpClient httpClient = new HttpClient())
                        {
                            httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.102 Safari/537.36");
                            var downloadresponse = await httpClient.GetAsync(downloadUrl);
                            if (downloadresponse.IsSuccessStatusCode)
                            {
                                var html = await downloadresponse.Content.ReadAsStringAsync();
                                await SaveProductFromJson(html);
                            }
                        }
                    }

                    productPrices.Add(productId, returnPrice);
                }
                else //add product if it doesn't exist
                {
                    var downloadUrl = BuildDownloadUrl(productId);
                    using (HttpClient httpClient = new HttpClient())
                    {
                        httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.102 Safari/537.36");
                        var downloadresponse = await httpClient.GetAsync(downloadUrl);
                        if (downloadresponse.IsSuccessStatusCode)
                        {
                            var html = await downloadresponse.Content.ReadAsStringAsync();
                            var prod = await SaveProductFromJson(html);
                            returnPrice = new ApiProd() { Price = 0, Url = "https://www.pollychron.com/Home/Details/" + prod.Id };

                        }
                    }
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

        public class ApiProd
        {
            public decimal Price { get; set; }
            public string Url { get; set; }
        }

        private const string TakealotApi = "https://api.takealot.com/rest/v-1-8-0/product-details";

        protected string BuildDownloadUrl(string productId)
        {
            return string.Concat(TakealotApi, "/", productId, "?platform=desktop");
        }

        private async Task<Data.Product> SaveProductFromJson(string httpResponse)
        {
            if (string.IsNullOrEmpty(httpResponse))
                return null;

            TakealotJson jsonObject = JsonConvert.DeserializeObject<TakealotJson>(httpResponse);
            bool hasPurchasePrice = !jsonObject.event_data.documents.product.purchase_price.HasValue;
            if (hasPurchasePrice)
                return null;

            decimal price = jsonObject.event_data.documents.product.purchase_price.Value;
            decimal? originalPrice = jsonObject.event_data.documents.product.original_price;

            if (price >= originalPrice)//prevent bad data
                originalPrice = null;

            Data.Product product = DataAccess.FetchProductOrDefault(jsonObject.data_layer.prodid);
            if (product != null)
            {
                var lastPrice = await DataAccess.FetchProductLastPrice(product.Id);
                if (lastPrice.Price == price)
                    return null;
                await DataAccess.SaveAsync(new PriceHistory(lastPrice, price, originalPrice) { ProductId = product.Id });
                return null;
            }

            product = new Data.Product()
            {
                UniqueIdentifier = jsonObject.data_layer.prodid
            };
            product.PriceHistory.Add(new PriceHistory(null, price, originalPrice) { ProductId = product.Id });

            product.Breadcrumb = jsonObject.breadcrumbs?.items.Select(x => x.name).Aggregate((i, j) => i + "," + j);
            product.Title = jsonObject.title;
            product.Description = jsonObject.description?.html;
            product.Category = jsonObject.data_layer.categoryname?.Select(x => x).Aggregate((i, j) => i + "," + j);
            if (jsonObject.gallery.images.Any())
                product.Image = jsonObject.gallery.images[0].Replace("{size}", "pdpxl");
            product.Url = jsonObject.desktop_href;
            product.LastChecked = jsonObject.meta.date_retrieved;

            await DataAccess.SaveAsync(product);

            return product;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
using Newtonsoft.Json;
using Polly.Data;
using Polly.Domain;
using Polly.Website.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Polly.Website
{
    public static class TopTenCache
    {
        public static int RepopulatedCount = 0;
        public static int FailedPopulateAttempts = 0;
        public const string Top10 = "Top10";
        public static List<IndexProductView> Products
        {
            get
            {
                //first time populated must be manual
                if (!LastPopulated.HasValue)
                    return new List<IndexProductView>();

                var cacheObject = HttpContext.Current.Cache.Get(Top10);
                if (cacheObject is List<IndexProductView> top10)
                {
                    return top10;
                }
                else
                {
                    return new List<IndexProductView>();
                }
            }
        }

        public static DateTime? LastPopulated { get; set; }
        private static int RetryCount = 0;

        public static async Task SetCacheItems(IEnumerable<ProductIdAndPrice> productIds)
        {
            List<IndexProductView> _products = new List<IndexProductView>();
            var biggestDiscountProducts = await DataAccess.GetTopDiscountProducts(productIds);
            foreach (var prod in biggestDiscountProducts)
            {
                _products.Add(new IndexProductView()
                {
                    DiscountPercentage = prod.Discount,
                    ImageSrc = prod.ImageSrc,
                    PriceBoarLink = prod.PriceBoarLink,
                    SellingPrice = prod.SellingPrice.ToString(),
                    TakealotLink = prod.TakealotLink,
                    Title = prod.Title
                });
            }
            HttpContext.Current.Cache.Add(Top10, _products, null, DateTime.Today.AddDays(1).Date, System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Default, null);
            RepopulatedCount++;
            LastPopulated = DateTime.Now;
        }

        public static async Task PopulateTopTenCache()
        {
            //reset retry count if a new day
            if (LastPopulated?.Date != DateTime.Now.Date)
                RetryCount = 0;

            if (RetryCount > 5)
                return;

            try
            {
                List<ProductIdAndPrice> productIds = new List<ProductIdAndPrice>();

                using (var download = new TakealotDownloader())
                {
                    var takealotPromotionsResponse = await download.DownloadAsync("https://api.takealot.com/rest/v-1-9-0/promotions?is_bundle_included=True");
                    TakealotPromotionJson promotions = JsonConvert.DeserializeObject<TakealotPromotionJson>(takealotPromotionsResponse);
                    if (promotions.status_code == 200)
                    {
                        var dailyDealPromotionId = promotions.response.FirstOrDefault(x => x.display_name.Equals("Daily Deals", StringComparison.InvariantCultureIgnoreCase))?.promotion_id;
                        if (dailyDealPromotionId == null)
                            throw new Exception("No active daily dails available in promotions");
                        int start = 0;
                        int max = 100;

                        var dailyDeals = await download.DownloadAsync($"https://api.takealot.com/rest/v-1-9-0/productlines/search?sort=BestSelling%20Descending&rows=100&daily_deals_rows=100&start={start}&detail=listing&filter=Available:true&filter=Promotions:{dailyDealPromotionId}");
                        TakealotProductLine productLine = JsonConvert.DeserializeObject<TakealotProductLine>(dailyDeals);
                        productIds.AddRange(productLine.results.productlines.Select(x => new ProductIdAndPrice() { UniqueIdentifier = x.uuid, SellingPrice = x.selling_price }));

                        max = productLine.results.num_found;
                        while (start < max)
                        {
                            start += 100;
                            dailyDeals = await download.DownloadAsync($"https://api.takealot.com/rest/v-1-9-0/productlines/search?sort=BestSelling%20Descending&rows=100&daily_deals_rows=100&start={start}&detail=listing&filter=Available:true&filter=Promotions:{dailyDealPromotionId}");
                            productLine = JsonConvert.DeserializeObject<TakealotProductLine>(dailyDeals);
                            productIds.AddRange(productLine.results.productlines.Select(x => new ProductIdAndPrice() { UniqueIdentifier = x.uuid, SellingPrice = x.selling_price }));
                        }
                    }
                }

                await SetCacheItems(productIds);
            }
            catch (Exception e)
            {
                RetryCount++;
                FailedPopulateAttempts++;
                await DataAccess.LogError(e);
                throw;
            }
        }
    }
}
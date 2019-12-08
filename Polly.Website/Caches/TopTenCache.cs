﻿using Newtonsoft.Json;
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
        public static List<IndexProductView> Products = new List<IndexProductView>();
        private static DateTime _lastPopulateAttempt = DateTime.Now;

        private static int RetryCount = 0;
        public static async Task PopulateTopTenCache()
        {
            Products = new List<IndexProductView>();

            //reset retry count if a new day
            if (_lastPopulateAttempt.Date != DateTime.Now.Date)
                RetryCount = 0;

            if (RetryCount > 5)
                return;

            _lastPopulateAttempt = DateTime.Now;
            try
            {
                List<ProductIdAndPrice> productIds = new List<ProductIdAndPrice>();
                using (var download = new TakealotDownloader())
                {
                    var takealotPromotionsResponse = await download.DownloadAsync("https://api.takealot.com/rest/v-1-9-0/promotions?is_bundle_included=True");
                    TakealotPromotionJson promotions = JsonConvert.DeserializeObject<TakealotPromotionJson>(takealotPromotionsResponse);
                    if (promotions.status_code == 200)
                    {
                        var dailyDealPromotionId = promotions.response.First(x => x.display_name == "Daily Deals" && x.is_active).promotion_id;
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

                var biggestDiscountProducts = await DataAccess.GetTopDiscountProducts(productIds);
                foreach (var prod in biggestDiscountProducts)
                {
                    Products.Add(new IndexProductView()
                    {
                        DiscountPercentage = prod.Discount,
                        ImageSrc = prod.ImageSrc,
                        PriceBoarLink = prod.PriceBoarLink,
                        SellingPrice = prod.SellingPrice.ToString(),
                        TakealotLink = prod.TakealotLink,
                        Title = prod.Title
                    });
                }
            }
            catch
            {
                RetryCount++;
                //fail silently
            }
        }
    }    
}
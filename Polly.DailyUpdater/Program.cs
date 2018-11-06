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
    class Program
    {
        private const string TakealotApiPromition = "https://api.takealot.com/rest/v-1-8-0/productlines/search?sort=BestSelling%20Descending&rows=100&daily_deals_rows=100&start=0&detail=listing&filter=Available:true&filter=Promotions:56232"; //56222
        private const string TakealotApiPromitionInterpolation = "https://api.takealot.com/rest/v-1-8-0/productlines/search?sort=BestSelling%20Descending&rows=100&daily_deals_rows=100&start={0}&detail=listing&filter=Available:true&filter=Promotions:56232";
        private const string TakealotProductDetails = "https://api.takealot.com/rest/v-1-7-0/product-details/";

        static void Main(string[] args)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/65.0.3325.181 Safari/537.36");
            var responseString = httpClient.GetStringAsync(TakealotApiPromition).Result;
            var jsonObject = JsonConvert.DeserializeObject<TakealotJsonPromotion>(responseString);
            int specialCount = jsonObject.results.num_found;
            int step = 0;
            do
            {
                foreach (var product in jsonObject.results.productlines)
                    DataAccess.AddToDownloadQueue(TakealotProductDetails + product.uuid, 1, 2);

                step += 100;
                responseString = httpClient.GetStringAsync(string.Format(TakealotApiPromitionInterpolation, step)).Result;
                jsonObject = JsonConvert.DeserializeObject<TakealotJsonPromotion>(responseString);
            }
            while (step < specialCount);
        }
    }
}

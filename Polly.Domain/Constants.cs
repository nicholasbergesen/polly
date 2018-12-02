using System;
using System.Collections.Generic;
using System.Text;

namespace Polly.Domain
{
    public static class Constants
    {
        public static class Takealot
        {
            public const string ApiPromition = "https://api.takealot.com/rest/v-1-8-0/promotions";
            public const string ApiPromitionProduct = "https://api.takealot.com/rest/v-1-8-0/productlines/search?filter=Promotions:{0}&rows={1}&start={2}";
            public const string ProductDetails = "https://api.takealot.com/rest/v-1-7-0/product-details/{0}?platform=desktop";
            public const string UserAgent = "User-Agent";
            public const string UserAgentValue = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/65.0.3325.181 Safari/537.36";
        }
    }
}

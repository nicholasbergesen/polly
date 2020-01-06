using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polly.Domain
{
    public class TakealotProductLine
    {
        public string uuid { get; set; }
        public Results results { get; set; }
        public Params parameters { get; set; }

        public class Entry
        {
            public string filter { get; set; }
            public string name { get; set; }
            public string display_name { get; set; }
            public int num_docs { get; set; }
            public string start { get; set; }
            public string end { get; set; }
            public string id { get; set; }
            public string type { get; set; }
            public string value { get; set; }
            public string badge_url_pattern { get; set; }
        }

        public class Facet
        {
            public string name { get; set; }
            public string display_name { get; set; }
            public string type { get; set; }
            public IList<Entry> entries { get; set; }
        }

        public class Filter
        {
            public string filter { get; set; }
            public string title { get; set; }
            public int num_docs { get; set; }
            public string display_name { get; set; }
            public string display_value { get; set; }
            public string type { get; set; }
            public string name { get; set; }
        }

        public class Cover
        {
            public string url { get; set; }
            public string filename { get; set; }
            public int modified { get; set; }
            public string key { get; set; }
            public int size { get; set; }
        }

        public class PriceRange
        {
            public decimal min { get; set; }
            public decimal max { get; set; }
        }

        public class WebPriceRange
        {
            public decimal min { get; set; }
            public decimal max { get; set; }
        }

        public class PriceRangeOld
        {
            public decimal min { get; set; }
            public decimal max { get; set; }
        }

        public class Rating
        {
            public int count { get; set; }
            public double average { get; set; }
        }

        public class DailydealQty
        {
        }

        public class Availability
        {
            public int cpt { get; set; }
            public int jhb { get; set; }
            public int min { get; set; }
            public int max { get; set; }
            public bool supplier { get; set; }
            public bool seller { get; set; }
        }

        public class Product
        {
            public int id { get; set; }
            public bool ispreorder { get; set; }
        }

        public class Seller
        {
            public int? id { get; set; }
            public string name { get; set; }
        }

        public class Format
        {
            public int idFormatType { get; set; }
            public string type { get; set; }
            public int id { get; set; }
            public string name { get; set; }
        }

        public class Author
        {
            public int idAuthor { get; set; }
            public string author { get; set; }
        }

        public class Dates
        {
            public string expected { get; set; }
            public string preorder { get; set; }
            public string released { get; set; }
        }

        public class Promotion
        {
            public int id { get; set; }
            public int deal_id { get; set; }
            public string name { get; set; }
            public string slug { get; set; }
            public int start { get; set; }
            public int end { get; set; }
            public bool active { get; set; }
            public object department { get; set; }
            public string landingpage { get; set; }
            public bool showflyout { get; set; }
            public bool showtopnav { get; set; }
            public bool showsidebanner { get; set; }
            public bool showleftnav { get; set; }
            public bool showtimer { get; set; }
            public object color { get; set; }
            public double price { get; set; }
            public int group_id { get; set; }
            public int qty { get; set; }
            public int position { get; set; }
            public string display_name { get; set; }
            public object promotion_price { get; set; }
            public int promotion_qualifying_quantity { get; set; }
            public int product_qualifying_quantity { get; set; }
            public bool is_lead_time_allowed { get; set; }
            public double supplier_subsidy { get; set; }
            public double marketing_subsidy { get; set; }
        }

        public class PromotionsQty
        {
        }

        public class Preselect
        {
            public string key { get; set; }
            public int? val { get; set; }
        }

        public class Variant
        {
            public string type { get; set; }
            public string label { get; set; }
        }

        public class ServerInfo
        {
            public string data_timestamp { get; set; }
            public string server { get; set; }
            public int ts { get; set; }
        }

        public class Buybox
        {
            public double min_price { get; set; }
            public int total_results { get; set; }
            public bool is_takealot { get; set; }
            public int sku_id { get; set; }
        }

        public class AppEntry
        {
            public string id { get; set; }
            public string type { get; set; }
            public string value { get; set; }
            public string badge_url_pattern { get; set; }
        }

        public class Badges
        {
            public IList<Entry> entries { get; set; }
            public IList<AppEntry> app_entries { get; set; }
            public int? promotion_id { get; set; }
        }

        public class Views
        {
            public Badges badges { get; set; }
        }

        public class SubConditionTypes
        {
            public int unboxed { get; set; }
        }

        public class BuyboxUsed
        {
            public double min_price { get; set; }
            public int total_results { get; set; }
            public bool is_takealot { get; set; }
            public int sku_id { get; set; }
            public SubConditionTypes sub_condition_types { get; set; }
        }

        public class Productline
        {
            public int id { get; set; }
            public string uuid { get; set; }
            public string title { get; set; }
            public string subtitle { get; set; }
            public Cover cover { get; set; }
            public bool active { get; set; }
            public bool available { get; set; }
            public PriceRange priceRange { get; set; }
            public WebPriceRange webPriceRange { get; set; }
            public PriceRangeOld priceRangeOld { get; set; }
            public decimal selling_price { get; set; }
            public double web_selling_price { get; set; }
            public double old_selling_price { get; set; }
            public bool promote_price { get; set; }
            public bool has_skus { get; set; }
            public Rating rating { get; set; }
            public int reviews { get; set; }
            public bool dailydeal { get; set; }
            public DailydealQty dailydeal_qty { get; set; }
            public object soldout_daily_deals { get; set; }
            public Availability availability { get; set; }
            public bool quotable { get; set; }
            public bool exclusive { get; set; }
            public bool prepaid { get; set; }
            public bool voucher { get; set; }
            public bool ebook { get; set; }
            public bool colourvariants { get; set; }
            public Product product { get; set; }
            public int? sellerlistingid { get; set; }
            public Seller seller { get; set; }
            public IList<Format> formats { get; set; }
            public IList<Author> authors { get; set; }
            public IList<int> departments { get; set; }
            public Dates dates { get; set; }
            public IList<Promotion> promotions { get; set; }
            public PromotionsQty promotions_qty { get; set; }
            public string saving { get; set; }
            public string web_saving { get; set; }
            public Preselect preselect { get; set; }
            public string uri { get; set; }
            public bool has_variants { get; set; }
            public IList<Variant> variants { get; set; }
            public ServerInfo server_info { get; set; }
            public Buybox buybox { get; set; }
            public Views views { get; set; }
            public BuyboxUsed buybox_used { get; set; }
        }

        public class Results
        {
            public int start { get; set; }
            public string next_is_after { get; set; }
            public string previous_is_before { get; set; }
            public int num_found { get; set; }
            public IList<Facet> facets { get; set; }
            public IList<Filter> filters { get; set; }
            public IList<object> breadcrumbs { get; set; }
            public IList<Productline> productlines { get; set; }
        }

        public class Params
        {
            public IList<string> version { get; set; }
            public IList<string> sort { get; set; }
            public IList<string> rows { get; set; }
            public IList<string> start { get; set; }
            public IList<string> detail { get; set; }
            public IList<string> filter { get; set; }
        }

        public string backend { get; set; }
    }
}
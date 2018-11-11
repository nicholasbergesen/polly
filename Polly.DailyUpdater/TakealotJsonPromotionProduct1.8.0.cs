namespace Polly.DailyUpdater
{
    public class TakealotPromotionProduct
    {
        public string backend { get; set; }
        public Params _params { get; set; }
        public string uuid { get; set; }
        public Results results { get; set; }
    }

    public class Params
    {
        public string[] filter { get; set; }
        public string[] return_breadcrumb_enabled { get; set; }
        public string[] return_filters_enabled { get; set; }
        public string[] version { get; set; }
        public string[] return_facets_enabled { get; set; }
    }

    public class Results
    {
        public object[] breadcrumbs { get; set; }
        public Facet[] facets { get; set; }
        public int start { get; set; }
        public Productline[] productlines { get; set; }
        public int num_found { get; set; }
        public Filter[] filters { get; set; }
    }

    public class Facet
    {
        public string display_name { get; set; }
        public string type { get; set; }
        public string name { get; set; }
        public Entry[] entries { get; set; }
    }

    public class Entry
    {
        public string filter { get; set; }
        public string display_name { get; set; }
        public string name { get; set; }
        public int num_docs { get; set; }
        public string start { get; set; }
        public string end { get; set; }
    }

    public class Productline
    {
        public bool exclusive { get; set; }
        public int? sku_id { get; set; }
        public string subtitle { get; set; }
        public Web_Price_Range web_price_range { get; set; }
        public Image image { get; set; }
        public int selling_price { get; set; }
        public int web_selling_price { get; set; }
        public string saving { get; set; }
        public int stock_cpt { get; set; }
        public string id { get; set; }
        public object sellerlistingid { get; set; }
        public Buybox buybox { get; set; }
        public bool? is_liquor { get; set; }
        public string title { get; set; }
        public bool? is_ebook { get; set; }
        public Promotions_Qty promotions_qty { get; set; }
        public int old_selling_price { get; set; }
        public bool stock_supplier { get; set; }
        public bool is_preorder { get; set; }
        public bool is_active { get; set; }
        public int stock_jhb { get; set; }
        public int non_promo_selling_price { get; set; }
        public bool has_skus { get; set; }
        public Promo[] promos { get; set; }
        public int stock_on_hand { get; set; }
        public bool is_available { get; set; }
        public object[] authors { get; set; }
        public bool dailydeal { get; set; }
        public object[] variants { get; set; }
        public bool is_prepaid { get; set; }
        public string promotions { get; set; }
        public string web_saving { get; set; }
        public object seller_id { get; set; }
        public float star_rating { get; set; }
        public object seller_name { get; set; }
        public bool is_voucher { get; set; }
        public bool has_variants { get; set; }
        public string uri { get; set; }
        public Price_Range price_range { get; set; }
        public object[] formats { get; set; }
        public string date_released { get; set; }
        public Shipping_Information shipping_information { get; set; }
    }

    public class Web_Price_Range
    {
    }

    public class Image
    {
        public string small { get; set; }
        public string large { get; set; }
        public string listgrid { get; set; }
        public string full { get; set; }
        public string fb { get; set; }
    }

    public class Buybox
    {
        public int total_results { get; set; }
        public float min_price { get; set; }
        public bool is_takealot { get; set; }
        public int sku_id { get; set; }
    }

    public class Promotions_Qty
    {
        public int _56253 { get; set; }
        public int _50967 { get; set; }
        public int _55894 { get; set; }
    }

    public class Price_Range
    {
    }

    public class Shipping_Information
    {
        public bool in_stock { get; set; }
        public string _string { get; set; }
        public string[] stock_warehouses { get; set; }
    }

    public class Promo
    {
        public int product_qualifying_quantity { get; set; }
        public string end { get; set; }
        public int deal_id { get; set; }
        public float price { get; set; }
        public string slug { get; set; }
        public int qty { get; set; }
        public string start { get; set; }
        public string short_display_name { get; set; }
        public int id { get; set; }
        public int position { get; set; }
        public int group_id { get; set; }
        public int promotion_qualifying_quantity { get; set; }
        public object promotion_price { get; set; }
        public string display_name { get; set; }
    }

    public class Filter
    {
        public string display_name { get; set; }
        public string name { get; set; }
        public int num_docs { get; set; }
        public string title { get; set; }
        public string filter { get; set; }
        public string type { get; set; }
        public object display_value { get; set; }
    }
}

namespace Polly.DailyUpdater
{

    public class TakealotJsonPromotion
    {
        public string backend { get; set; }
        public Params _params { get; set; }
        public string uuid { get; set; }
        public Results results { get; set; }
    }

    public class Params
    {
        public string[] sort { get; set; }
        public string[] return_breadcrumb_enabled { get; set; }
        public string[] return_filters_enabled { get; set; }
        public string[] rows { get; set; }
        public string[] detail { get; set; }
        public string[] filter { get; set; }
        public string[] start { get; set; }
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
        public Rating rating { get; set; }
        public string subtitle { get; set; }
        public bool exclusive { get; set; }
        public bool colourvariants { get; set; }
        public bool voucher { get; set; }
        public float selling_price { get; set; }
        public float web_selling_price { get; set; }
        public string saving { get; set; }
        public Seller seller { get; set; }
        public Availability availability { get; set; }
        public int? sellerlistingid { get; set; }
        public Pricerangeold priceRangeOld { get; set; }
        public string uuid { get; set; }
        public string title { get; set; }
        public bool quotable { get; set; }
        public int id { get; set; }
        public Promotions_Qty promotions_qty { get; set; }
        public Pricerange priceRange { get; set; }
        public Dailydeal_Qty dailydeal_qty { get; set; }
        public object soldout_daily_deals { get; set; }
        public float old_selling_price { get; set; }
        public bool promote_price { get; set; }
        public bool available { get; set; }
        public Product product { get; set; }
        public bool prepaid { get; set; }
        public Webpricerange webPriceRange { get; set; }
        public bool ebook { get; set; }
        public bool has_skus { get; set; }
        public Preselect preselect { get; set; }
        public Author[] authors { get; set; }
        public bool dailydeal { get; set; }
        public Variant[] variants { get; set; }
        public bool active { get; set; }
        public Buybox buybox { get; set; }
        public Promotion[] promotions { get; set; }
        public string web_saving { get; set; }
        public Dates dates { get; set; }
        public bool has_variants { get; set; }
        public Server_Info server_info { get; set; }
        public Cover cover { get; set; }
        public string uri { get; set; }
        public int[] departments { get; set; }
        public int reviews { get; set; }
        public Format[] formats { get; set; }
    }

    public class Rating
    {
        public int count { get; set; }
        public float average { get; set; }
    }

    public class Seller
    {
        public int? id { get; set; }
        public string name { get; set; }
    }

    public class Availability
    {
        public bool supplier { get; set; }
        public int max { get; set; }
        public int jhb { get; set; }
        public int cpt { get; set; }
        public int min { get; set; }
        public bool seller { get; set; }
    }

    public class Pricerangeold
    {
        public float max { get; set; }
        public float min { get; set; }
    }

    public class Promotions_Qty
    {
        public int _56222 { get; set; }
        public int _55890 { get; set; }
        public int _56107 { get; set; }
        public int _50967 { get; set; }
        public int _55843 { get; set; }
        public int _56172 { get; set; }
    }

    public class Pricerange
    {
        public float max { get; set; }
        public float min { get; set; }
    }

    public class Dailydeal_Qty
    {
        public int _1254066 { get; set; }
        public int _1254061 { get; set; }
        public int _1254059 { get; set; }
        public int _1254067 { get; set; }
        public int _1254063 { get; set; }
        public int _1254060 { get; set; }
        public int _1254331 { get; set; }
        public int _1254065 { get; set; }
        public int _1254064 { get; set; }
        public int _1253755 { get; set; }
        public int _1253751 { get; set; }
        public int _1253763 { get; set; }
        public int _1253745 { get; set; }
        public int _1253767 { get; set; }
        public int _1253840 { get; set; }
        public int _1253914 { get; set; }
        public int _1254068 { get; set; }
        public int _1254038 { get; set; }
        public int _1254039 { get; set; }
        public int _1253945 { get; set; }
        public int _1253979 { get; set; }
        public int _1253956 { get; set; }
        public int _1253955 { get; set; }
        public int _1253954 { get; set; }
        public int _1253860 { get; set; }
        public int _1253861 { get; set; }
        public int _1253753 { get; set; }
        public int _1254047 { get; set; }
        public int _1254032 { get; set; }
        public int _1253843 { get; set; }
        public int _1253752 { get; set; }
        public int _1253765 { get; set; }
        public int _1253775 { get; set; }
        public int _1254083 { get; set; }
        public int _1253735 { get; set; }
        public int _1253783 { get; set; }
        public int _1254082 { get; set; }
        public int _1254049 { get; set; }
        public int _1253920 { get; set; }
        public int _1254289 { get; set; }
        public int _1254033 { get; set; }
        public int _1254056 { get; set; }
        public int _1254051 { get; set; }
        public int _1253999 { get; set; }
        public int _1254008 { get; set; }
        public int _1253922 { get; set; }
        public int _1253781 { get; set; }
        public int _1254054 { get; set; }
        public int _1254015 { get; set; }
        public int _1254053 { get; set; }
        public int _1253959 { get; set; }
        public int _1253770 { get; set; }
        public int _1253779 { get; set; }
        public int _1253916 { get; set; }
        public int _1253917 { get; set; }
        public int _1253721 { get; set; }
        public int _1253758 { get; set; }
        public int _1254048 { get; set; }
        public int _1253964 { get; set; }
        public int _1254104 { get; set; }
        public int _1253747 { get; set; }
        public int _1253788 { get; set; }
        public int _1253778 { get; set; }
        public int _1253792 { get; set; }
        public int _1253793 { get; set; }
        public int _1253736 { get; set; }
        public int _1253911 { get; set; }
        public int _1253950 { get; set; }
        public int _1253871 { get; set; }
        public int _1253776 { get; set; }
        public int _1254296 { get; set; }
        public int _1253845 { get; set; }
        public int _1253915 { get; set; }
        public int _1253918 { get; set; }
        public int _1253872 { get; set; }
        public int _1254258 { get; set; }
        public int _1254081 { get; set; }
        public int _1254050 { get; set; }
        public int _1254044 { get; set; }
        public int _1253729 { get; set; }
        public int _1254091 { get; set; }
        public int _1254036 { get; set; }
        public int _1254280 { get; set; }
        public int _1254271 { get; set; }
        public int _1254113 { get; set; }
        public int _1254259 { get; set; }
        public int _1253858 { get; set; }
        public int _1253951 { get; set; }
        public int _1254035 { get; set; }
        public int _1254042 { get; set; }
        public int _1254266 { get; set; }
        public int _1254094 { get; set; }
        public int _1254299 { get; set; }
        public int _1254009 { get; set; }
        public int _1254010 { get; set; }
    }

    public class Product
    {
        public bool ispreorder { get; set; }
        public int id { get; set; }
    }

    public class Webpricerange
    {
        public float max { get; set; }
        public float min { get; set; }
    }

    public class Preselect
    {
    }

    public class Buybox
    {
        public int total_results { get; set; }
        public float min_price { get; set; }
        public bool is_takealot { get; set; }
        public int sku_id { get; set; }
    }

    public class Dates
    {
        public string expected { get; set; }
        public string preorder { get; set; }
        public string released { get; set; }
    }

    public class Server_Info
    {
        public string server { get; set; }
        public int ts { get; set; }
        public string data_timestamp { get; set; }
    }

    public class Cover
    {
        public string url { get; set; }
        public int size { get; set; }
        public int modified { get; set; }
        public string key { get; set; }
        public string filename { get; set; }
    }

    public class Author
    {
        public int idAuthor { get; set; }
        public string author { get; set; }
    }

    public class Variant
    {
        public string type { get; set; }
        public string label { get; set; }
    }

    public class Promotion
    {
        public int deal_id { get; set; }
        public object color { get; set; }
        public int qty { get; set; }
        public bool showleftnav { get; set; }
        public int id { get; set; }
        public int product_qualifying_quantity { get; set; }
        public int group_id { get; set; }
        public string display_name { get; set; }
        public bool showtopnav { get; set; }
        public int start { get; set; }
        public object department { get; set; }
        public bool showflyout { get; set; }
        public bool showsidebanner { get; set; }
        public float price { get; set; }
        public bool is_lead_time_allowed { get; set; }
        public float marketing_subsidy { get; set; }
        public bool active { get; set; }
        public int end { get; set; }
        public string slug { get; set; }
        public bool showtimer { get; set; }
        public string name { get; set; }
        public int promotion_qualifying_quantity { get; set; }
        public float supplier_subsidy { get; set; }
        public int position { get; set; }
        public string landingpage { get; set; }
        public object promotion_price { get; set; }
    }

    public class Format
    {
        public int idFormatType { get; set; }
        public string type { get; set; }
        public int id { get; set; }
        public string name { get; set; }
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

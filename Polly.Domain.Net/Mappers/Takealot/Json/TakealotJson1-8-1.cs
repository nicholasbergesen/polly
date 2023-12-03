using System;

namespace Polly.Domain
{
    public class TakealotJson
    {
        public Exchanges_And_Returns exchanges_and_returns { get; set; }
        public Stock_Availability stock_availability { get; set; }
        public Meta meta { get; set; }
        public Seo seo { get; set; }
        public Buybox buybox { get; set; }
        public string title { get; set; }
        public Breadcrumbs breadcrumbs { get; set; }
        public Customers_Also_Bought customers_also_bought { get; set; }
        public Top_Navigation top_navigation { get; set; }
        public Product_Information product_information { get; set; }
        public Data_Layer data_layer { get; set; }
        public string desktop_href { get; set; }
        public Event_Data event_data { get; set; }
        public Core core { get; set; }
        public Sharing sharing { get; set; }
        public Description description { get; set; }
        public Facebook_Opengraph facebook_opengraph { get; set; }
        public Bullet_Point_Attributes bullet_point_attributes { get; set; }
        public Google_Structured_Data google_structured_data { get; set; }
        public Gallery gallery { get; set; }
        public Reviews reviews { get; set; }
        public Frequently_Bought_Together frequently_bought_together { get; set; }
    }

    public class Exchanges_And_Returns
    {
        public string copy { get; set; }
        public string tab_title { get; set; }
    }

    public class Stock_Availability
    {
        public string status { get; set; }
        public bool is_leadtime { get; set; }
        public object distribution_centres { get; set; }
        public object seasonal_message_info { get; set; }
        public object when_do_i_get_it_text { get; set; }
        public bool is_imported { get; set; }
        public bool display_seasonal_message { get; set; }
        public object seasonal_message_text { get; set; }
        public object when_do_i_get_it_info { get; set; }
    }

    public class Meta
    {
        public string identifier { get; set; }
        public string href { get; set; }
        public DateTime date_retrieved { get; set; }
        public string type { get; set; }
        public bool display { get; set; }
    }

    public class Seo
    {
        public Alternate alternate { get; set; }
        public string canonical { get; set; }
        public string description { get; set; }
        public string title { get; set; }
    }

    public class Alternate
    {
        public string android { get; set; }
        public string handheld { get; set; }
    }

    public class Buybox
    {
        public Loyalty_Prices[] loyalty_prices { get; set; }
        public object promotion_qty { get; set; }
        public int? product_id { get; set; }
        public string add_to_cart_text { get; set; }
        public int product_line_id { get; set; }
        public bool is_add_to_wishlist_available { get; set; }
        public object variants_call_to_action { get; set; }
        public object multibuy_label { get; set; }
        public object is_free_shipping_available { get; set; }
        public bool multibuy_display { get; set; }
        public bool? is_add_to_cart_available { get; set; }
        public float[] prices { get; set; }
        public decimal? listing_price { get; set; }
        public object promotion_qty_display_text { get; set; }
    }

    public class Loyalty_Prices
    {
        public string display_text { get; set; }
        public decimal[] prices { get; set; }
        public object info_mode { get; set; }
        public string id { get; set; }
        public object description { get; set; }
    }

    public class Breadcrumbs
    {
        public Item[] items { get; set; }
    }

    public class Item
    {
        public string type { get; set; }
        public string slug { get; set; }
        public string name { get; set; }
        public int id { get; set; }
    }

    public class Customers_Also_Bought
    {
        public string href { get; set; }
    }

    public class Top_Navigation
    {
        public Item1[] items { get; set; }
    }

    public class Item1
    {
        public string foreground { get; set; }
        public object[] font_styling { get; set; }
        public string text { get; set; }
        public string href { get; set; }
        public string background { get; set; }
        public string id { get; set; }
    }

    public class Product_Information
    {
        public Item2[] items { get; set; }
        public string tab_title { get; set; }
        public object categories { get; set; }
    }

    public class Item2
    {
        public object displayable_text { get; set; }
        public string display_name { get; set; }
        public object value { get; set; }
        public string source { get; set; }
        public string content_type { get; set; }
        public string type { get; set; }
        public object default_display_value { get; set; }
        public string id { get; set; }
    }

    public class Data_Layer
    {
        public int? sku { get; set; }
        public decimal? totalPrice { get; set; }
        public string name { get; set; }
        public string productlineSku { get; set; }
        public string departmentname { get; set; }
        public int[] categoryid { get; set; }
        public int? departmentid { get; set; }
        public string pageType { get; set; }
        public int quantity { get; set; }
        public string _event { get; set; }
        public string prodid { get; set; }
        public string[] categoryname { get; set; }
    }

    public class Event_Data
    {
        public Documents documents { get; set; }
    }

    public class Documents
    {
        public Product product { get; set; }
    }

    public class Product
    {
        public decimal? original_price { get; set; }
        public int sku_id { get; set; }
        public int product_line_id { get; set; }
        public decimal? purchase_price { get; set; }
        public bool in_stock { get; set; }
        public bool market_place_listing { get; set; }
        public string lead_time { get; set; }
    }

    public class Core
    {
        public object star_rating { get; set; }
        public object subtitle { get; set; }
        public string title { get; set; }
        public object brand { get; set; }
        public Author[] authors { get; set; }
        public int id { get; set; }
        public int? reviews { get; set; }
        public Format[] formats { get; set; }
        public string slug { get; set; }
    }

    public class Author
    {
        public int idAuthor { get; set; }
        public string author { get; set; }
    }

    public class Format
    {
        public int? idFormatType { get; set; }
        public string type { get; set; }
        public int? id { get; set; }
        public string name { get; set; }
    }

    public class Sharing
    {
        public string url { get; set; }
        public Copy copy { get; set; }
        public string[] enabled { get; set; }
    }

    public class Copy
    {
        public Body body { get; set; }
        public Subject subject { get; set; }
    }

    public class Body
    {
        public string twitter { get; set; }
        public string googleplus { get; set; }
        public string facebook { get; set; }
        public string email { get; set; }
        public string pinterest { get; set; }
    }

    public class Subject
    {
        public string email { get; set; }
    }

    public class Description
    {
        public string tab_title { get; set; }
        public string html { get; set; }
    }

    public class Facebook_Opengraph
    {
        public string ogurl { get; set; }
        public string ogsite_name { get; set; }
        public string ogtype { get; set; }
        public string ogdescription { get; set; }
        public string ogimage { get; set; }
    }

    public class Bullet_Point_Attributes
    {
        public Item3[] items { get; set; }
    }

    public class Item3
    {
        public string description { get; set; }
        public bool positive { get; set; }
        public string text { get; set; }
        public string info_mode { get; set; }
        public string type { get; set; }
        public string id { get; set; }
    }

    public class Google_Structured_Data
    {
        public Item4[] items { get; set; }
    }

    public class Item4
    {
        public string context { get; set; }
        public string type { get; set; }
        public Target target { get; set; }
        public string name { get; set; }
        public Image[] image { get; set; }
        public Additionalproperty[] additionalProperty { get; set; }
        public Offer[] offers { get; set; }
    }

    public class Target
    {
        public string type { get; set; }
        public string urlTemplate { get; set; }
    }

    public class Image
    {
        public string contentUrl { get; set; }
        public string type { get; set; }
    }

    public class Additionalproperty
    {
        public string type { get; set; }
        public string value { get; set; }
        public string name { get; set; }
    }

    public class Offer
    {
        public decimal price { get; set; }
        public string type { get; set; }
        public string priceCurrency { get; set; }
    }

    public class Gallery
    {
        public string[] images { get; set; }
        public string size_guide_href { get; set; }
    }

    public class Reviews
    {
        public int? count { get; set; }
        public string tab_title { get; set; }
        public object star_rating { get; set; }
        public string href { get; set; }
    }

    public class Frequently_Bought_Together
    {
        public string href { get; set; }
    }

}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polly.Downloader
{
    public class TakealotJson
    {
        public class WebPriceRange
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
            public double min_price { get; set; }
            public bool is_takealot { get; set; }
            public int sku_id { get; set; }
        }

        public class Type
        {
            public string slug { get; set; }
            public int id { get; set; }
            public string image { get; set; }
            public string name { get; set; }
        }

        public class Format
        {
            public string typename { get; set; }
            public int type { get; set; }
            public int id { get; set; }
            public string name { get; set; }
        }

        public class Attributes
        {
            public object grade { get; set; }
            public List<object> academic_institution { get; set; }
            public List<object> platforms { get; set; }
            public object shape { get; set; }
            public object legshape { get; set; }
            public List<object> lengthtype { get; set; }
            public List<object> bindings { get; set; }
            public object age_group { get; set; }
            public object size { get; set; }
            public List<object> regions { get; set; }
            public object fit { get; set; }
            public object pattern { get; set; }
            public List<object> colours { get; set; }
            public List<object> styletype { get; set; }
            public object colour_variant { get; set; }
            public object rise { get; set; }
            public object shoe_size { get; set; }
            public object neckline { get; set; }
            public object lens { get; set; }
            public List<object> characters { get; set; }
            public List<object> genders { get; set; }
            public object fastening { get; set; }
            public object heelheight { get; set; }
            public List<object> languages { get; set; }
            public object watchtype { get; set; }
            public List<Format> formats { get; set; }
            public object occasion { get; set; }
        }

        public class ShippingInformation
        {
            public bool in_stock { get; set; }
            public string @string { get; set; }
            public List<string> stock_warehouses { get; set; }
        }

        public class Sku
        {
            public bool exclusive { get; set; }
            public object saving { get; set; }
            public int tsin_id { get; set; }
            public int web_selling_price { get; set; }
            public int selling_price { get; set; }
            public List<Image> images { get; set; }
            public bool is_ebook { get; set; }
            public int stock_cpt { get; set; }
            public object size { get; set; }
            public Buybox buybox { get; set; }
            public int id { get; set; }
            public object shoe_size { get; set; }
            public int old_selling_price { get; set; }
            public bool stock_supplier { get; set; }
            public Type type { get; set; }
            public bool is_active { get; set; }
            public object seller_id { get; set; }
            public int stock_on_hand { get; set; }
            public bool is_available { get; set; }
            public object web_saving { get; set; }
            public int stock_jhb { get; set; }
            public object seller_name { get; set; }
            public int total_offers { get; set; }
            public Attributes attributes { get; set; }
            public ShippingInformation shipping_information { get; set; }
        }

        public class Image2
        {
            public string small { get; set; }
            public string large { get; set; }
            public string listgrid { get; set; }
            public string full { get; set; }
            public string fb { get; set; }
        }

        public class EligiblePaymentMethod
        {
        }

        public class Category
        {
            public int dept { get; set; }
            public int level { get; set; }
            public int id { get; set; }
            public int? parent { get; set; }
            public string name { get; set; }
        }

        public class Type2
        {
            public string slug { get; set; }
            public int id { get; set; }
            public string image { get; set; }
            public string name { get; set; }
        }

        public class PriceRange
        {
        }

        public class ShippingInformation2
        {
            public bool in_stock { get; set; }
            public string @string { get; set; }
            public List<string> stock_warehouses { get; set; }
        }

        public class Meta
        {
            public string Warranty { get; set; }
            public string Rating { get; set; }
            //public int Discs { get; set; }
            public string Zone { get; set; }
            public string Format { get; set; }
            public string Country { get; set; }
            public string Time { get; set; }
            public string Barcode { get; set; }
            public string Languages { get; set; }
            public string Genre { get; set; }
            public string Discs { get; set; }
            public string Studio { get; set; }
            public string Title { get; set; }
            public int Year { get; set; }
            //public int Time { get; set; }
            public List<string> Actors { get; set; }
        }

        public class Image3
        {
            public string small { get; set; }
            public string large { get; set; }
            public string listgrid { get; set; }
            public string full { get; set; }
            public string fb { get; set; }
        }

        public class Response
        {
            public bool exclusive { get; set; }
            public object subtitle { get; set; }
            public WebPriceRange web_price_range { get; set; }
            public List<Sku> skus { get; set; }
            public Image2 image { get; set; }
            public List<EligiblePaymentMethod> eligible_payment_methods { get; set; }
            public bool dailydeal { get; set; }
            public int web_selling_price { get; set; }
            public object saving { get; set; }
            public int stock_cpt { get; set; }
            public bool is_liquor { get; set; }
            public string title { get; set; }
            public bool is_ebook { get; set; }
            public List<Category> categories { get; set; }
            public bool stock_supplier { get; set; }
            public Type2 type { get; set; }
            public bool is_preorder { get; set; }
            public bool requires_tv_license { get; set; }
            public object brand { get; set; }
            public bool is_active { get; set; }
            public bool is_available { get; set; }
            public string description_text { get; set; }
            public PriceRange price_range { get; set; }
            public bool is_prepaid { get; set; }
            public int stock_jhb { get; set; }
            public double star_rating { get; set; }
            public List<object> classes { get; set; }
            public int total_offers { get; set; }
            public ShippingInformation2 shipping_information { get; set; }
            public Meta meta { get; set; }
            public int selling_price { get; set; }
            public List<Image3> images { get; set; }
            public bool has_available_skus { get; set; }
            public string id { get; set; }
            public bool is_on_special { get; set; }
            public bool show_mobicred_instalment { get; set; }
            public int old_selling_price { get; set; }
            public string description { get; set; }
            public bool has_skus { get; set; }
            public int stock_on_hand { get; set; }
            public List<object> deals_sold_out { get; set; }
            public List<object> promotions { get; set; }
            public object web_saving { get; set; }
            public bool is_voucher { get; set; }
            public string uri { get; set; }
            public List<object> selectors { get; set; }
            public string date_released { get; set; }
        }

        public class RootObject
        {
            public int status_code { get; set; }
            public string result { get; set; }
            public Response response { get; set; }
        }
    }
}

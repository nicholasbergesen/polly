namespace QuickType
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class TakealotJson
    {
        [JsonProperty("exchanges_and_returns")]
        public ExchangesAndReturns ExchangesAndReturns { get; set; }

        [JsonProperty("other_offers")]
        public OtherOffers OtherOffers { get; set; }

        [JsonProperty("stock_availability")]
        public TakealotJsonStockAvailability StockAvailability { get; set; }

        [JsonProperty("media_region")]
        public MediaRegion MediaRegion { get; set; }

        [JsonProperty("meta")]
        public Meta Meta { get; set; }

        [JsonProperty("seo")]
        public Seo Seo { get; set; }

        [JsonProperty("buybox")]
        public Buybox Buybox { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("breadcrumbs")]
        public Breadcrumbs Breadcrumbs { get; set; }

        [JsonProperty("customers_also_bought")]
        public CustomersAlsoBought CustomersAlsoBought { get; set; }

        [JsonProperty("top_navigation")]
        public TopNavigation TopNavigation { get; set; }

        [JsonProperty("product_information")]
        public ProductInformation ProductInformation { get; set; }

        [JsonProperty("data_layer")]
        public DataLayer DataLayer { get; set; }

        [JsonProperty("desktop_href")]
        public Uri DesktopHref { get; set; }

        [JsonProperty("event_data")]
        public EventData EventData { get; set; }

        [JsonProperty("core")]
        public Core Core { get; set; }

        [JsonProperty("sharing")]
        public Sharing Sharing { get; set; }

        [JsonProperty("description")]
        public Description Description { get; set; }

        [JsonProperty("facebook_opengraph")]
        public Dictionary<string, string> FacebookOpengraph { get; set; }

        [JsonProperty("bullet_point_attributes")]
        public BulletPointAttributes BulletPointAttributes { get; set; }

        [JsonProperty("google_structured_data")]
        public GoogleStructuredData GoogleStructuredData { get; set; }

        [JsonProperty("gallery")]
        public Gallery Gallery { get; set; }

        [JsonProperty("reviews")]
        public Reviews Reviews { get; set; }

        [JsonProperty("frequently_bought_together")]
        public CustomersAlsoBought FrequentlyBoughtTogether { get; set; }
    }

    public partial class Breadcrumbs
    {
        [JsonProperty("items")]
        public PurpleItem[] Items { get; set; }
    }

    public partial class PurpleItem
    {
        [JsonProperty("slug", NullValueHandling = NullValueHandling.Ignore)]
        public string Slug { get; set; }

        [JsonProperty("type")]
        public Categoryname Type { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("idFormatType", NullValueHandling = NullValueHandling.Ignore)]
        public long? IdFormatType { get; set; }
    }

    public partial class BulletPointAttributes
    {
        [JsonProperty("items")]
        public BulletPointAttributesItem[] Items { get; set; }
    }

    public partial class BulletPointAttributesItem
    {
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("positive")]
        public bool Positive { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("info_mode")]
        public string InfoMode { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }

    public partial class Buybox
    {
        [JsonProperty("loyalty_prices")]
        public LoyaltyPrice[] LoyaltyPrices { get; set; }

        [JsonProperty("promotion_qty")]
        public object PromotionQty { get; set; }

        [JsonProperty("product_id")]
        public long ProductId { get; set; }

        [JsonProperty("add_to_cart_text")]
        public string AddToCartText { get; set; }

        [JsonProperty("product_line_id")]
        public long ProductLineId { get; set; }

        [JsonProperty("is_add_to_wishlist_available")]
        public bool IsAddToWishlistAvailable { get; set; }

        [JsonProperty("variants_call_to_action")]
        public object VariantsCallToAction { get; set; }

        [JsonProperty("multibuy_label")]
        public object MultibuyLabel { get; set; }

        [JsonProperty("is_free_shipping_available")]
        public bool IsFreeShippingAvailable { get; set; }

        [JsonProperty("multibuy_display")]
        public bool MultibuyDisplay { get; set; }

        [JsonProperty("is_add_to_cart_available")]
        public bool IsAddToCartAvailable { get; set; }

        [JsonProperty("prices")]
        public double[] Prices { get; set; }

        [JsonProperty("listing_price")]
        public double ListingPrice { get; set; }

        [JsonProperty("promotion_qty_display_text")]
        public object PromotionQtyDisplayText { get; set; }
    }

    public partial class LoyaltyPrice
    {
        [JsonProperty("display_text")]
        public string DisplayText { get; set; }

        [JsonProperty("prices")]
        public long[] Prices { get; set; }

        [JsonProperty("info_mode")]
        public object InfoMode { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("description")]
        public object Description { get; set; }
    }

    public partial class Core
    {
        [JsonProperty("star_rating")]
        public object StarRating { get; set; }

        [JsonProperty("subtitle")]
        public string Subtitle { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("brand")]
        public string Brand { get; set; }

        [JsonProperty("authors")]
        public object Authors { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("reviews")]
        public long Reviews { get; set; }

        [JsonProperty("formats")]
        public PurpleItem[] Formats { get; set; }

        [JsonProperty("slug")]
        public string Slug { get; set; }
    }

    public partial class CustomersAlsoBought
    {
        [JsonProperty("href")]
        public Uri Href { get; set; }
    }

    public partial class DataLayer
    {
        [JsonProperty("sku")]
        public long Sku { get; set; }

        [JsonProperty("totalPrice")]
        public double TotalPrice { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("productlineSku")]
        public string ProductlineSku { get; set; }

        [JsonProperty("departmentname")]
        public string Departmentname { get; set; }

        [JsonProperty("categoryid")]
        public long[] Categoryid { get; set; }

        [JsonProperty("departmentid")]
        public long Departmentid { get; set; }

        [JsonProperty("pageType")]
        public string PageType { get; set; }

        [JsonProperty("quantity")]
        public long Quantity { get; set; }

        [JsonProperty("event")]
        public string Event { get; set; }

        [JsonProperty("prodid")]
        public string Prodid { get; set; }

        [JsonProperty("categoryname")]
        public Categoryname[] Categoryname { get; set; }
    }

    public partial class Description
    {
        [JsonProperty("tab_title")]
        public string TabTitle { get; set; }

        [JsonProperty("html")]
        public string Html { get; set; }
    }

    public partial class EventData
    {
        [JsonProperty("documents")]
        public Documents Documents { get; set; }
    }

    public partial class Documents
    {
        [JsonProperty("product")]
        public Product Product { get; set; }
    }

    public partial class Product
    {
        [JsonProperty("original_price", NullValueHandling = NullValueHandling.Ignore)]
        public long? OriginalPrice { get; set; }

        [JsonProperty("sku_id")]
        public long SkuId { get; set; }

        [JsonProperty("product_line_id")]
        public long ProductLineId { get; set; }

        [JsonProperty("market_place_listing")]
        public bool MarketPlaceListing { get; set; }

        [JsonProperty("in_stock")]
        public bool InStock { get; set; }

        [JsonProperty("purchase_price")]
        public double PurchasePrice { get; set; }

        [JsonProperty("lead_time")]
        public string LeadTime { get; set; }
    }

    public partial class ExchangesAndReturns
    {
        [JsonProperty("copy")]
        public string Copy { get; set; }

        [JsonProperty("tab_title")]
        public string TabTitle { get; set; }
    }

    public partial class Gallery
    {
        [JsonProperty("images")]
        public string[] Images { get; set; }

        [JsonProperty("size_guide_href")]
        public Uri SizeGuideHref { get; set; }
    }

    public partial class GoogleStructuredData
    {
        [JsonProperty("items")]
        public GoogleStructuredDataItem[] Items { get; set; }
    }

    public partial class GoogleStructuredDataItem
    {
        [JsonProperty("@context")]
        public Uri Context { get; set; }

        [JsonProperty("@type")]
        public string Type { get; set; }

        [JsonProperty("target", NullValueHandling = NullValueHandling.Ignore)]
        public Target Target { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("image", NullValueHandling = NullValueHandling.Ignore)]
        public Image[] Image { get; set; }

        [JsonProperty("brand", NullValueHandling = NullValueHandling.Ignore)]
        public Brand Brand { get; set; }

        [JsonProperty("additionalProperty", NullValueHandling = NullValueHandling.Ignore)]
        public AdditionalProperty[] AdditionalProperty { get; set; }

        [JsonProperty("offers", NullValueHandling = NullValueHandling.Ignore)]
        public Offer[] Offers { get; set; }
    }

    public partial class AdditionalProperty
    {
        [JsonProperty("@type")]
        public string Type { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public partial class Brand
    {
        [JsonProperty("logo")]
        public Uri Logo { get; set; }

        [JsonProperty("@type")]
        public string Type { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public partial class Image
    {
        [JsonProperty("contentUrl")]
        public Uri ContentUrl { get; set; }

        [JsonProperty("@type")]
        public string Type { get; set; }
    }

    public partial class Offer
    {
        [JsonProperty("price")]
        public double Price { get; set; }

        [JsonProperty("@type")]
        public string Type { get; set; }

        [JsonProperty("priceCurrency")]
        public string PriceCurrency { get; set; }
    }

    public partial class Target
    {
        [JsonProperty("@type")]
        public string Type { get; set; }

        [JsonProperty("urlTemplate")]
        public string UrlTemplate { get; set; }
    }

    public partial class MediaRegion
    {
        [JsonProperty("regions")]
        [JsonConverter(typeof(DecodeArrayConverter))]
        public long[] Regions { get; set; }

        [JsonProperty("copy")]
        public MediaRegionCopy Copy { get; set; }

        [JsonProperty("tab_title")]
        public string TabTitle { get; set; }
    }

    public partial class MediaRegionCopy
    {
        [JsonProperty("2")]
        public string The2 { get; set; }
    }

    public partial class Meta
    {
        [JsonProperty("identifier")]
        public string Identifier { get; set; }

        [JsonProperty("href")]
        public Uri Href { get; set; }

        [JsonProperty("date_retrieved")]
        public DateTimeOffset DateRetrieved { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("display")]
        public bool Display { get; set; }
    }

    public partial class OtherOffers
    {
        [JsonProperty("conditions")]
        public Condition[] Conditions { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }
    }

    public partial class Condition
    {
        [JsonProperty("count")]
        public long Count { get; set; }

        [JsonProperty("items")]
        public ConditionItem[] Items { get; set; }

        [JsonProperty("from_price")]
        public double FromPrice { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("condition")]
        public string ConditionCondition { get; set; }
    }

    public partial class ConditionItem
    {
        [JsonProperty("product_id")]
        public long ProductId { get; set; }

        [JsonProperty("event_data")]
        public EventData EventData { get; set; }

        [JsonProperty("price")]
        public double Price { get; set; }

        [JsonProperty("stock_availability")]
        public ItemStockAvailability StockAvailability { get; set; }

        [JsonProperty("seller")]
        public Seller Seller { get; set; }

        [JsonProperty("is_takealot")]
        public bool IsTakealot { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }

    public partial class Seller
    {
        [JsonProperty("fulfilled_by_takealot")]
        public object FulfilledByTakealot { get; set; }

        [JsonProperty("seller_id")]
        public long SellerId { get; set; }

        [JsonProperty("display_name")]
        public string DisplayName { get; set; }
    }

    public partial class ItemStockAvailability
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("is_leadtime")]
        public bool IsLeadtime { get; set; }

        [JsonProperty("evt_status")]
        public string EvtStatus { get; set; }

        [JsonProperty("distribution_centres")]
        public object DistributionCentres { get; set; }

        [JsonProperty("when_do_i_get_it_text")]
        public string WhenDoIGetItText { get; set; }

        [JsonProperty("is_imported")]
        public bool IsImported { get; set; }

        [JsonProperty("display")]
        public bool Display { get; set; }

        [JsonProperty("is_in_stock")]
        public bool IsInStock { get; set; }

        [JsonProperty("when_do_i_get_it_info")]
        public string WhenDoIGetItInfo { get; set; }
    }

    public partial class ProductInformation
    {
        [JsonProperty("items")]
        public ProductInformationItem[] Items { get; set; }

        [JsonProperty("tab_title")]
        public string TabTitle { get; set; }

        [JsonProperty("categories")]
        public object Categories { get; set; }
    }

    public partial class ProductInformationItem
    {
        [JsonProperty("displayable_text")]
        public DisplayableText DisplayableText { get; set; }

        [JsonProperty("display_name")]
        public string DisplayName { get; set; }

        [JsonProperty("value")]
        public ItemValue Value { get; set; }

        [JsonProperty("source")]
        public Source Source { get; set; }

        [JsonProperty("content_type")]
        public ContentType ContentType { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("default_display_value")]
        public object DefaultDisplayValue { get; set; }

        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }
    }

    public partial class ValueClass
    {
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public TypeClass Type { get; set; }

        [JsonProperty("period", NullValueHandling = NullValueHandling.Ignore)]
        public Period Period { get; set; }

        [JsonProperty("object", NullValueHandling = NullValueHandling.Ignore)]
        public Object Object { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public long? Id { get; set; }
    }

    public partial class Object
    {
        [JsonProperty("image")]
        public Uri Image { get; set; }

        [JsonProperty("department_ids")]
        public long[] DepartmentIds { get; set; }
    }

    public partial class Period
    {
        [JsonProperty("value")]
        public long Value { get; set; }

        [JsonProperty("unit")]
        public string Unit { get; set; }
    }

    public partial class TypeClass
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }
    }

    public partial class Reviews
    {
        [JsonProperty("count")]
        public long Count { get; set; }

        [JsonProperty("tab_title")]
        public string TabTitle { get; set; }

        [JsonProperty("star_rating")]
        public object StarRating { get; set; }

        [JsonProperty("href")]
        public Uri Href { get; set; }
    }

    public partial class Seo
    {
        [JsonProperty("alternate")]
        public Alternate Alternate { get; set; }

        [JsonProperty("canonical")]
        public Uri Canonical { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }
    }

    public partial class Alternate
    {
        [JsonProperty("android")]
        public string Android { get; set; }

        [JsonProperty("handheld")]
        public Uri Handheld { get; set; }
    }

    public partial class Sharing
    {
        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("copy")]
        public SharingCopy Copy { get; set; }

        [JsonProperty("enabled")]
        public string[] Enabled { get; set; }
    }

    public partial class SharingCopy
    {
        [JsonProperty("body")]
        public Body Body { get; set; }

        [JsonProperty("subject")]
        public Subject Subject { get; set; }
    }

    public partial class Body
    {
        [JsonProperty("twitter")]
        public string Twitter { get; set; }

        [JsonProperty("googleplus")]
        public string Googleplus { get; set; }

        [JsonProperty("facebook")]
        public string Facebook { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("pinterest")]
        public string Pinterest { get; set; }
    }

    public partial class Subject
    {
        [JsonProperty("email")]
        public string Email { get; set; }
    }

    public partial class TakealotJsonStockAvailability
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("is_leadtime")]
        public bool IsLeadtime { get; set; }

        [JsonProperty("distribution_centres")]
        public object DistributionCentres { get; set; }

        [JsonProperty("when_do_i_get_it_text")]
        public string WhenDoIGetItText { get; set; }

        [JsonProperty("is_imported")]
        public bool IsImported { get; set; }

        [JsonProperty("when_do_i_get_it_info")]
        public string WhenDoIGetItInfo { get; set; }
    }

    public partial class TopNavigation
    {
        [JsonProperty("items")]
        public TopNavigationItem[] Items { get; set; }
    }

    public partial class TopNavigationItem
    {
        [JsonProperty("foreground")]
        public string Foreground { get; set; }

        [JsonProperty("font_styling")]
        public object[] FontStyling { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("href")]
        public string Href { get; set; }

        [JsonProperty("background")]
        public string Background { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }

    public enum Categoryname { Category, Department, Movies };

    public enum ContentType { TextPlain };

    public enum Source { Categories, ProductlineMeta, TsinAttributeService };

    public partial struct DisplayableText
    {
        public long? Integer;
        public string String;
        public string[] StringArray;

        public static implicit operator DisplayableText(long Integer) => new DisplayableText { Integer = Integer };
        public static implicit operator DisplayableText(string String) => new DisplayableText { String = String };
        public static implicit operator DisplayableText(string[] StringArray) => new DisplayableText { StringArray = StringArray };
        public bool IsNull => StringArray == null && Integer == null && String == null;
    }

    public partial struct ValueElement
    {
        public PurpleItem[] PurpleItemArray;
        public string String;

        public static implicit operator ValueElement(PurpleItem[] PurpleItemArray) => new ValueElement { PurpleItemArray = PurpleItemArray };
        public static implicit operator ValueElement(string String) => new ValueElement { String = String };
    }

    public partial struct ItemValue
    {
        public ValueElement[] AnythingArray;
        public long? Integer;
        public string String;
        public ValueClass ValueClass;

        public static implicit operator ItemValue(ValueElement[] AnythingArray) => new ItemValue { AnythingArray = AnythingArray };
        public static implicit operator ItemValue(long Integer) => new ItemValue { Integer = Integer };
        public static implicit operator ItemValue(string String) => new ItemValue { String = String };
        public static implicit operator ItemValue(ValueClass ValueClass) => new ItemValue { ValueClass = ValueClass };
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters = {
                CategorynameConverter.Singleton,
                ContentTypeConverter.Singleton,
                DisplayableTextConverter.Singleton,
                SourceConverter.Singleton,
                ItemValueConverter.Singleton,
                ValueElementConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class CategorynameConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Categoryname) || t == typeof(Categoryname?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "Movies":
                    return Categoryname.Movies;
                case "category":
                    return Categoryname.Category;
                case "department":
                    return Categoryname.Department;
            }
            throw new Exception("Cannot unmarshal type Categoryname");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Categoryname)untypedValue;
            switch (value)
            {
                case Categoryname.Movies:
                    serializer.Serialize(writer, "Movies");
                    return;
                case Categoryname.Category:
                    serializer.Serialize(writer, "category");
                    return;
                case Categoryname.Department:
                    serializer.Serialize(writer, "department");
                    return;
            }
            throw new Exception("Cannot marshal type Categoryname");
        }

        public static readonly CategorynameConverter Singleton = new CategorynameConverter();
    }

    internal class DecodeArrayConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(long[]);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            reader.Read();
            var value = new List<long>();
            while (reader.TokenType != JsonToken.EndArray)
            {
                var converter = ParseStringConverter.Singleton;
                var arrayItem = (long)converter.ReadJson(reader, typeof(long), null, serializer);
                value.Add(arrayItem);
                reader.Read();
            }
            return value.ToArray();
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            var value = (long[])untypedValue;
            writer.WriteStartArray();
            foreach (var arrayItem in value)
            {
                var converter = ParseStringConverter.Singleton;
                converter.WriteJson(writer, arrayItem, serializer);
            }
            writer.WriteEndArray();
            return;
        }

        public static readonly DecodeArrayConverter Singleton = new DecodeArrayConverter();
    }

    internal class ParseStringConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(long) || t == typeof(long?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            long l;
            if (Int64.TryParse(value, out l))
            {
                return l;
            }
            throw new Exception("Cannot unmarshal type long");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (long)untypedValue;
            serializer.Serialize(writer, value.ToString());
            return;
        }

        public static readonly ParseStringConverter Singleton = new ParseStringConverter();
    }

    internal class ContentTypeConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(ContentType) || t == typeof(ContentType?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            if (value == "text/plain")
            {
                return ContentType.TextPlain;
            }
            throw new Exception("Cannot unmarshal type ContentType");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (ContentType)untypedValue;
            if (value == ContentType.TextPlain)
            {
                serializer.Serialize(writer, "text/plain");
                return;
            }
            throw new Exception("Cannot marshal type ContentType");
        }

        public static readonly ContentTypeConverter Singleton = new ContentTypeConverter();
    }

    internal class DisplayableTextConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(DisplayableText) || t == typeof(DisplayableText?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            switch (reader.TokenType)
            {
                case JsonToken.Null:
                    return new DisplayableText { };
                case JsonToken.Integer:
                    var integerValue = serializer.Deserialize<long>(reader);
                    return new DisplayableText { Integer = integerValue };
                case JsonToken.String:
                case JsonToken.Date:
                    var stringValue = serializer.Deserialize<string>(reader);
                    return new DisplayableText { String = stringValue };
                case JsonToken.StartArray:
                    var arrayValue = serializer.Deserialize<string[]>(reader);
                    return new DisplayableText { StringArray = arrayValue };
            }
            throw new Exception("Cannot unmarshal type DisplayableText");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            var value = (DisplayableText)untypedValue;
            if (value.IsNull)
            {
                serializer.Serialize(writer, null);
                return;
            }
            if (value.Integer != null)
            {
                serializer.Serialize(writer, value.Integer.Value);
                return;
            }
            if (value.String != null)
            {
                serializer.Serialize(writer, value.String);
                return;
            }
            if (value.StringArray != null)
            {
                serializer.Serialize(writer, value.StringArray);
                return;
            }
            throw new Exception("Cannot marshal type DisplayableText");
        }

        public static readonly DisplayableTextConverter Singleton = new DisplayableTextConverter();
    }

    internal class SourceConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Source) || t == typeof(Source?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "categories":
                    return Source.Categories;
                case "productline-meta":
                    return Source.ProductlineMeta;
                case "tsin-attribute-service":
                    return Source.TsinAttributeService;
            }
            throw new Exception("Cannot unmarshal type Source");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Source)untypedValue;
            switch (value)
            {
                case Source.Categories:
                    serializer.Serialize(writer, "categories");
                    return;
                case Source.ProductlineMeta:
                    serializer.Serialize(writer, "productline-meta");
                    return;
                case Source.TsinAttributeService:
                    serializer.Serialize(writer, "tsin-attribute-service");
                    return;
            }
            throw new Exception("Cannot marshal type Source");
        }

        public static readonly SourceConverter Singleton = new SourceConverter();
    }

    internal class ItemValueConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(ItemValue) || t == typeof(ItemValue?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            switch (reader.TokenType)
            {
                case JsonToken.Integer:
                    var integerValue = serializer.Deserialize<long>(reader);
                    return new ItemValue { Integer = integerValue };
                case JsonToken.String:
                case JsonToken.Date:
                    var stringValue = serializer.Deserialize<string>(reader);
                    return new ItemValue { String = stringValue };
                case JsonToken.StartObject:
                    var objectValue = serializer.Deserialize<ValueClass>(reader);
                    return new ItemValue { ValueClass = objectValue };
                case JsonToken.StartArray:
                    var arrayValue = serializer.Deserialize<ValueElement[]>(reader);
                    return new ItemValue { AnythingArray = arrayValue };
            }
            throw new Exception("Cannot unmarshal type ItemValue");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            var value = (ItemValue)untypedValue;
            if (value.Integer != null)
            {
                serializer.Serialize(writer, value.Integer.Value);
                return;
            }
            if (value.String != null)
            {
                serializer.Serialize(writer, value.String);
                return;
            }
            if (value.AnythingArray != null)
            {
                serializer.Serialize(writer, value.AnythingArray);
                return;
            }
            if (value.ValueClass != null)
            {
                serializer.Serialize(writer, value.ValueClass);
                return;
            }
            throw new Exception("Cannot marshal type ItemValue");
        }

        public static readonly ItemValueConverter Singleton = new ItemValueConverter();
    }

    internal class ValueElementConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(ValueElement) || t == typeof(ValueElement?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            switch (reader.TokenType)
            {
                case JsonToken.String:
                case JsonToken.Date:
                    var stringValue = serializer.Deserialize<string>(reader);
                    return new ValueElement { String = stringValue };
                case JsonToken.StartArray:
                    var arrayValue = serializer.Deserialize<PurpleItem[]>(reader);
                    return new ValueElement { PurpleItemArray = arrayValue };
            }
            throw new Exception("Cannot unmarshal type ValueElement");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            var value = (ValueElement)untypedValue;
            if (value.String != null)
            {
                serializer.Serialize(writer, value.String);
                return;
            }
            if (value.PurpleItemArray != null)
            {
                serializer.Serialize(writer, value.PurpleItemArray);
                return;
            }
            throw new Exception("Cannot marshal type ValueElement");
        }

        public static readonly ValueElementConverter Singleton = new ValueElementConverter();
    }
}

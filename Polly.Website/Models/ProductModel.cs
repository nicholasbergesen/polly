using Polly.Data;
using System.ComponentModel.DataAnnotations;

namespace Polly.Website.Models
{
    public class ProductModel
    {
        public long Id { get; }
        public string UniqueIdentifier { get; }

        [DataType(DataType.Url)]
        public string Url { get; }
        public string LastChecked { get; }
        public string Title { get; }

        [DataType(DataType.Html)]
        public string Description { get; }
        public string Breadcrumb { get; }
        public string Category { get; }

        [DataType(DataType.ImageUrl)]
        public string Image { get; }

        public PriceHistoriesModel PriceHistory { get; private set; }

        public DealModel Deal { get; private set; }

        private ProductModel(Product product)
        {
            Id = product.Id;
            UniqueIdentifier = product.UniqueIdentifier;
            Url = product.Url;
            LastChecked = product.LastChecked.ToLongDateString();
            Title = product.Title;
            Description = product.Description;
            Breadcrumb = product.Breadcrumb;
            Category = product.Category;
            Image = product.Image;
        }

        public static ProductModel Create(Product product)
        {
            var model = new ProductModel(product);
            var priceHistory = PriceHistoriesModel.Create(product.PriceHistory);
            model.PriceHistory = priceHistory;
            //model.Deal = DealModel.Create(priceHistory);
            return model;
        }
    }
}
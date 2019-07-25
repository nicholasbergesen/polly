using Polly.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Polly.Website.Models
{
    public class ProductModel
    {
        public long Id { get; set; }
        public string UniqueIdentifier { get; set; }

        [DataType(DataType.Url)]
        public string Url { get; set; }
        public string LastChecked { get; set; }
        public string Title { get; set; }

        [DataType(DataType.Html)]
        public string Description { get; set; }
        public string Breadcrumb { get; set; }
        public string Category { get; set; }

        [DataType(DataType.ImageUrl)]
        public string Image { get; set; }
        public List<PriceHistoryModel> PriceHistory { get; set; }

        public ProductModel(Product product)
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

            PriceHistory = product.PriceHistory
                .Select(item => new PriceHistoryModel(item))
                .OrderByDescending(x => x.TimeStamp)
                .ToList();
        }
    }
}
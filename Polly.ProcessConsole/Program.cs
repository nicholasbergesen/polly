using HtmlAgilityPack;
using Newtonsoft.Json;
using Polly.Data;
using Polly.Downloader;
using System;
using System.Linq;
using Product = Polly.Data.Product;

namespace Polly.ProcessConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var unprocessed = DataAccess.Unprocessed();
            foreach (var downloadData in unprocessed)
            {
                downloadData.ProcessDateTime = DateTime.Now;
                Product product;
                if (downloadData.Website.DataSourceType.Id == DataSourceTypeEnum.Html)
                {
                    product = ProcessHtml(downloadData);
                }
                else if (downloadData.Website.DataSourceType.Id == DataSourceTypeEnum.JSON)
                {
                    product = ProcessJson(downloadData);
                }
                else
                {
                    throw new Exception("Unknown dataSourceType");
                }
                DataAccess.SaveAsync(product).Wait();
                DataAccess.DeleteAsync(downloadData).Wait();
            }
        }

        private static Product ProcessHtml(DownloadData downloadData)
        {
            Product product = new Product();
            product.DownloadDataId = downloadData.Id;
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(downloadData.RawHtml);
            product.Name = htmlDocument.DocumentNode.SelectSingleNode(downloadData.Website.HeadingXPath).InnerText.Trim();
            product.Description = htmlDocument.DocumentNode.SelectSingleNode(downloadData.Website.DescriptionXPath).InnerText.Trim();
            if (decimal.TryParse(htmlDocument.DocumentNode.SelectSingleNode(downloadData.Website.PriceXPath).InnerText.Trim().Replace(",", ""), out decimal res))
                product.Price = res;
            product.ProductUniqueIdentifier = downloadData.Url.GetHashCode();
            if (downloadData.Website.BreadcrumbXPath != null) product.Breadcrumb = htmlDocument.DocumentNode.SelectSingleNode(downloadData.Website.BreadcrumbXPath)?.InnerText.Trim();
            if (downloadData.Website.CategoryXPath != null) product.Category = htmlDocument.DocumentNode.SelectSingleNode(downloadData.Website.CategoryXPath)?.InnerText.Trim();
            return product;
        }

        private static Product ProcessJson(DownloadData downloadData)
        {
            Product product = new Product();
            product.DownloadDataId = downloadData.Id;
            TakealotJson jsonObject = JsonConvert.DeserializeObject<TakealotJson>(downloadData.RawHtml);
            product.Breadcrumb = jsonObject.breadcrumbs.items.Select(x => x.name).Aggregate((i, j) => i + ", " + j);
            product.Name = jsonObject.title;
            product.Description = jsonObject.description?.html;
            product.Category = jsonObject.data_layer.categoryname.Select(x => x).Aggregate((i, j) => i + ", " + j);
            product.Price = jsonObject.data_layer.totalPrice;
            product.ProductUniqueIdentifier = jsonObject.data_layer.sku.GetHashCode();

            return product;
        }
    }
}

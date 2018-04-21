using HtmlAgilityPack;
using Newtonsoft.Json;
using Polly.Data;
using Polly.Downloader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.XPath;

namespace Polly.ProcessConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var unprocessed = DataAccess.Unprocessed(100);
            Parallel.ForEach(unprocessed, async (downloadData) => {
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

                DataAccess.SaveAsync(downloadData).Wait();
                DataAccess.SaveAsync(product).Wait();
            });
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
            product.UrlHash = downloadData.Url.GetHashCode();
            if (downloadData.Website.SubHeadingXPath != null) product.Subtitle = htmlDocument.DocumentNode.SelectSingleNode(downloadData.Website.SubHeadingXPath)?.InnerText.Trim();
            if (downloadData.Website.BreadcrumbXPath != null) product.Breadcrumb = htmlDocument.DocumentNode.SelectSingleNode(downloadData.Website.BreadcrumbXPath)?.InnerText.Trim();
            if (downloadData.Website.CategoryXPath != null) product.Category = htmlDocument.DocumentNode.SelectSingleNode(downloadData.Website.CategoryXPath)?.InnerText.Trim();
            return product;
        }

        private static Product ProcessJson(DownloadData downloadData)
        {
            Product product = new Product();
            product.DownloadDataId = downloadData.Id;
            TakealotJson jsonObject = JsonConvert.DeserializeObject<TakealotJson>(downloadData.RawHtml);

            product.Name = jsonObject.Response.title;
            product.Subtitle = jsonObject.Response.subtitle;
            product.Description = jsonObject.Response.description_text;
            product.Category = jsonObject.Response.categories.Select(x => x.name).Aggregate((i, j) => i + ", " + j);
            product.Price = jsonObject.Response.selling_price  / 100;
            product.UrlHash = downloadData.Url.GetHashCode();

            return product;
        }
    }
}

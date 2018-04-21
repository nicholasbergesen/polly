using HtmlAgilityPack;
using Polly.Data;
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
                if(downloadData.Website.BreadcrumbXPath != null) product.Breadcrumb = htmlDocument.DocumentNode.SelectSingleNode(downloadData.Website.BreadcrumbXPath)?.InnerText.Trim();
                if(downloadData.Website.CategoryXPath != null) product.Category = htmlDocument.DocumentNode.SelectSingleNode(downloadData.Website.CategoryXPath)?.InnerText.Trim();

                var saveDownloadTask = DataAccess.SaveAsync(downloadData);
                var saveProductTask = DataAccess.SaveAsync(product);

                await saveDownloadTask;
                await saveProductTask;
            });
        }
    }
}

using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PollyConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/65.0.3325.162 Safari/537.36");
            using (StreamReader sr = new StreamReader("input.txt"))
            {
                while (!sr.EndOfStream)
                {
                    var html = client.GetStringAsync(sr.ReadLine()).Result;
                    HtmlDocument htmlDocument = new HtmlDocument();
                    htmlDocument.LoadHtml(html);

                    HtmlNode productDataNode = htmlDocument.DocumentNode.SelectSingleNode("//div[@class='product-data left']/div[1]/div[1]");
                    string title = productDataNode.SelectSingleNode("//h1").InnerText.Trim();

                    HtmlNode amountNode = productDataNode.SelectSingleNode("//span[@class='amount']");
                    if (amountNode == null)
                        amountNode = htmlDocument.DocumentNode.SelectSingleNode("//div[@id='buybox']").SelectSingleNode("//span[@class='amount']");
                    string trimmed = amountNode.InnerText.Replace(",", "");
                    double price = double.Parse(trimmed);

                    File.AppendAllText("output.txt",$"{DateTime.Now}:{title} R{price}");
                }
            }
            Console.WriteLine("Done :D");
            Console.ReadLine();
        }
    }
}

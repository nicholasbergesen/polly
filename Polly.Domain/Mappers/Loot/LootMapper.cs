using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polly.Domain.Mappers.Loot
{
    class LootMapper
    {
        public HtmlMapper()
        {
        }

        public override Product MapContext(string htmlString)
        {

            try
            {
                //using (MemoryStream ms = new MemoryStream(Encoding.ASCII.GetBytes(htmlString)))
                //{
                //    XPathDocument xdoc = new XPathDocument(ms);
                //    if (downloadContext.CrawlData != null)
                //        UpdatePrimaryData(xdoc.CreateNavigator(), downloadContext.CrawlData);
                //}
            }
            catch 
            {
                
            }
        }
    }
}

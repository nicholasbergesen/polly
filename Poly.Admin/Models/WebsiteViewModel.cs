using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Poly.Admin.Models
{
    public class WebsiteViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Domain { get; set; }
        public string UserAgent { get; set; }
        public DateTime Schedule { get; set; }

        public string HeadingXPath { get; set; }
        public string SubHeadingXPath { get; set; }
        public string DescriptionXPath { get; set; }
        public string PriceXPath { get; set; }
        public string CategoryXPath { get; set; }
        public string BreadcrumbXPath { get; set; }
        public string MainImageXPath { get; set; }
    }
}
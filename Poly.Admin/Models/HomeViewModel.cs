using Polly.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Poly.Admin.Models
{
    public class HomeViewModel
    {
        public List<WebsiteModel> WebsiteModels { get; set; }

        private HomeViewModel(IEnumerable<Website> websites)
        {
            WebsiteModels = new List<WebsiteModel>();
            foreach (Website website in websites)
            {
                WebsiteModels.Add(new WebsiteModel()
                {
                    Id = website.Id,
                    Name = website.Name,
                    Domain = website.Domain,
                    UserAgent = website.UserAgent,
                    Schedule = website.Schedule,
                    HeadingXPath = website.HeadingXPath,
                    SubHeadingXPath = website.SubHeadingXPath,
                    DescriptionXPath = website.DescriptionXPath,
                    PriceXPath = website.PriceXPath,
                    CategoryXPath = website.CategoryXPath,
                    BreadcrumbXPath = website.BreadcrumbXPath,
                    MainImageXPath = website.MainImageXPath
                });
            }
        }

        public static HomeViewModel Create(IEnumerable<Website> websites)
        {
            return new HomeViewModel(websites);
        }
    }

    public class WebsiteModel
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
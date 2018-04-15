using Polly.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Poly.Admin.Models
{
    public class HomeViewModel
    {
        public List<WebsiteViewModel> WebsiteModels { get; set; }

        private HomeViewModel(IEnumerable<Website> websites)
        {
            WebsiteModels = new List<WebsiteViewModel>();
            foreach (Website website in websites)
            {
                WebsiteModels.Add(new WebsiteViewModel()
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
}
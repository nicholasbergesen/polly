using Polly.Data;
using Poly.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Poly.Admin.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            var websites = DataAccess.GetWebsites();
            return View(HomeViewModel.Create(websites));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(WebsiteViewModel websiteModel)
        {
            Website website = new Website()
            {
                Id = websiteModel.Id,
                Name = websiteModel.Name,
                Domain = websiteModel.Domain,
                UserAgent = websiteModel.UserAgent,
                Schedule = websiteModel.Schedule,
                HeadingXPath = websiteModel.HeadingXPath,
                SubHeadingXPath = websiteModel.SubHeadingXPath,
                DescriptionXPath = websiteModel.DescriptionXPath,
                PriceXPath = websiteModel.PriceXPath,
                CategoryXPath = websiteModel.CategoryXPath,
                BreadcrumbXPath = websiteModel.BreadcrumbXPath,
                MainImageXPath = websiteModel.MainImageXPath
            };
            await DataAccess.SaveWebsite(website);

            return RedirectToAction("Index");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
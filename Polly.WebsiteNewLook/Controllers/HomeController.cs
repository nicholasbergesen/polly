using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Polly.WebsiteNewLook.Controllers
{
    public class TempModel
    {
        public decimal Price { get; set; }
        public int Discount { get; set; }
        public string Title { get; set; }
        public string ImageSrc { get; set; }
    }

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
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
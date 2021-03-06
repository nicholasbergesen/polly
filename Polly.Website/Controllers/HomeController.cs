﻿using Polly.Data;
using Polly.Website.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
namespace Polly.Website.Controllers
{
    public class HomeController : Controller
    {
        private PollyDbContext db = new PollyDbContext();

        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
                return View(TopTenCache.Products);
            else
                return View(TopTenCache.Products.Take(5));
        }

        [HttpPost]
        public ActionResult Index(string searchString)
        {
            Product product = null;
            if (searchString.StartsWith("PLID"))
                product = (from p in db.Product
                           where p.UniqueIdentifier == searchString
                           select p).FirstOrDefault();
            else if (searchString.StartsWith("https://www.takealot.com"))
                product = (from p in db.Product
                           where p.Url == searchString
                           select p).FirstOrDefault();

            if (product != null)
                return RedirectToAction("Details", new { id = product.Id });
            else
                return View(new List<IndexProductView>());
        }

        public ActionResult Details(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Product product = (from prod in db.Product
                               where prod.Id == id
                               select prod)
                              .Include(x => x.PriceHistory)
                              .FirstOrDefault();

            if (product == null)
            {
                return HttpNotFound();
            }

            return View(ProductModel.Create(product));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Polly.Website.Core.Data;
using Polly.Website.Core.Models;
using System.Diagnostics;

namespace Polly.Website.Core.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _dbContext;

        public HomeController(ILogger<HomeController> logger, IDbContextFactory<ApplicationDbContext> dbContextFactory)
        {
            _logger = logger;
            _dbContext = dbContextFactory.CreateDbContext();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //old HomeController

        //public ActionResult Index()
        //{
        //    if (User.Identity.IsAuthenticated)
        //        return View(TopTenCache.Products);
        //    else
        //        return View(TopTenCache.Products.Take(5));
        //}

        //[HttpPost]
        //public ActionResult Index(string searchString)
        //{
        //    Product product = null;
        //    if (searchString.StartsWith("PLID"))
        //        product = (from p in db.Product
        //                   where p.UniqueIdentifier == searchString
        //                   select p).FirstOrDefault();
        //    else if (searchString.StartsWith("https://www.takealot.com"))
        //        product = (from p in db.Product
        //                   where p.Url == searchString
        //                   select p).FirstOrDefault();

        //    if (product != null)
        //        return RedirectToAction("Details", new { id = product.Id });
        //    else
        //        return View(new List<IndexProductView>());
        //}

        //public ActionResult Details(long? id)
        //{
        //    if (id == null)
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

        //    Product product = (from prod in db.Product
        //                       where prod.Id == id
        //                       select prod)
        //                      .Include(x => x.PriceHistory)
        //                      .FirstOrDefault();

        //    if (product == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    return View(ProductModel.Create(product));
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
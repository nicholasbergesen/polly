using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Polly.Data;
using Polly.Website.Models;

namespace Polly.Website.Controllers
{
    public class HomeController : Controller
    {
        private PollyDbContext db = new PollyDbContext();

        // GET: Products
        public async Task<ActionResult> Index()
        {
            var first = await db.Product.FirstAsync();
            var last = await db.Product.OrderByDescending(x => x.Id).FirstAsync();
            long[] numbers = new long[20];
            Random rand = new Random();
            for (int i = 0; i < numbers.Length; i++)
            {
                numbers[i] = (long)rand.Next((int)first.Id, (int)last.Id);
            }

            return View(await db.Product.Where(x => numbers.Contains(x.Id)).ToListAsync());
        }

        // GET: Discounts
        public ActionResult Discount()
        {
            var discountProducts = (from product in db.Product join
                                   priceHistory in db.PriceHistory on product.Id equals priceHistory.ProductId
                                   where priceHistory.OriginalPrice.HasValue
                                   select new DiscountProduct
                                   {
                                       Product = product,
                                       LastPrice = priceHistory
                                   })
                                   .Take(20).ToList();

            return View(discountProducts);
        }

        [HttpPost]
        public ActionResult Index(string searchString)
        {
            List<Product> products;
            if (searchString.StartsWith("PLID"))
                products = (from product in db.Product
                            where product.UniqueIdentifier == searchString
                            select product).Take(20).ToList();
            else if(searchString.StartsWith("https://www.takealot.com"))
                products = (from product in db.Product
                            where product.Url == searchString
                            select product).Take(20).ToList();
            else if(searchString.Count(x => x == ' ') > 1)
                products = (from product in db.Product
                            where product.Title.Substring(0, searchString.Length) == searchString
                            select product).Take(20).ToList();
            else
                products = (from product in db.Product
                            where product.Title.Contains(searchString)
                            select product).Take(20).ToList();

            if (products.Count == 1)
                return RedirectToAction("Details", new { id = products[0].Id });
            else
                return View(products);
        }

        // GET: Products/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = (from prod in db.Product
                               where prod.Id == id
                               select prod)
                              .Include(x => x.PriceHistory)
                              .FirstOrDefault();
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        //GET: Products/Create
        public ActionResult Create()
        {
            return View();
        }

        //POST: Products/Create
        //To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string product)
        {

            //if (ModelState.IsValid)
            //{
            //    db.Product.Add(product);
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}

            //return View(product);

            return View();
        }

        // GET: Products/Edit/5
        //public ActionResult Edit(long? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Product product = db.Product.Find(id);
        //    if (product == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(product);
        //}

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,UniqueIdentifierHash,UniqueIdentifier,Url,LastChecked,Title,Description,Breadcrumb,Category,Image")] Product product)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(product).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(product);
        //}

        // GET: Products/Delete/5
        //public ActionResult Delete(long? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Product product = db.Product.Find(id);
        //    if (product == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(product);
        //}

        // POST: Products/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(long id)
        //{
        //    Product product = db.Product.Find(id);
        //    db.Product.Remove(product);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

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

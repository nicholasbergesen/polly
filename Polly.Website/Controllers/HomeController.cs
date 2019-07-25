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
        public ActionResult Index()
        {
            //var t = await (from product in db.Product
            //              select (from price in product.PriceHistory
            //                      orderby price.TimeStamp descending
            //                      select price)
            //                      .FirstOrDefault())
            //               .FirstOrDefaultAsync();

            //var top20 = (from priceHistory in db.PriceHistory
            //             orderby priceHistory.DiscountPercentage descending
            //             select (from product in priceHistory.Product)

            //var top20ProductIds = db.PriceHistory
            //    .Where(x => x.DiscountPercentage.HasValue)
            //    .OrderBy(x => x.DiscountPercentage.Value)
            //    .Select(x => x.ProductId.Value)
            //    .Take(20);

            //var first = await db.Product.FirstAsync();
            //var last = await db.Product.OrderByDescending(x => x.Id).FirstAsync();
            //long[] numbers = new long[20];
            //Random rand = new Random();
            //for (int i = 0; i < numbers.Length; i++)
            //{
            //    numbers[i] = rand.Next((int)first.Id, (int)last.Id);
            //}
            //return View(await db.Product.Where(x => numbers.Contains(x.Id)).ToListAsync());
            //return View(await db.Product.Where(x => top20ProductIds.Contains(x.Id)).ToListAsync());


            //var first = await db.Product.FirstAsync();
            //var last = await db.Product.OrderByDescending(x => x.Id).FirstAsync();
            //long[] numbers = new long[20];
            //Random rand = new Random();
            //for (int i = 0; i < numbers.Length; i++)
            //{
            //    numbers[i] = rand.Next((int)first.Id, (int)last.Id);
            //}
            //return View(await db.Product.Where(x => numbers.Contains(x.Id)).ToListAsync());

            return View();
        }

        // GET: Discounts
        public async Task<ActionResult> Discount()
        {
            var discountProducts = await (from product in db.Product
                                    join priceHistory in db.PriceHistory on product.Id equals priceHistory.ProductId
                                    where priceHistory.OriginalPrice.HasValue
                                    orderby priceHistory.DiscountPercentage descending
                                    select new DiscountProduct
                                    {
                                        Product = product,
                                        LastPrice = priceHistory
                                    })
                                   .Take(20).ToListAsync();

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
            else if (searchString.StartsWith("https://www.takealot.com"))
                products = (from product in db.Product
                            where product.Url == searchString
                            select product).Take(20).ToList();
            else
                products = new List<Product>();
            //else if (searchString.Count(x => x == ' ') > 1)
            //    products = (from product in db.Product
            //                where product.Title.Substring(0, searchString.Length) == searchString
            //                select product).Take(20).ToList();
            //else
            //    products = (from product in db.Product
            //                where product.Title.Contains(searchString)
            //                select product).Take(20).ToList();

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

            return View(new ProductModel(product));
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

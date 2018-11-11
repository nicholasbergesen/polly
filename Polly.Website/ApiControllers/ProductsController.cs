using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Polly.Data;

namespace Polly.Website.Api.Controllers
{
    [RoutePrefix("api/product")]
    public class ProductsController : ApiController
    {
        private PollyDbContext db = new PollyDbContext();

        [HttpGet]
        [ResponseType(typeof(decimal))]
        [Route("{productId}")]
        public async Task<decimal> GetLastProductPrice(string productId)
        {
            return await (from product in db.Product
                          where product.UniqueIdentifier == productId
                          select (from price in product.PriceHistory
                                  orderby price.TimeStamp descending
                                  select price.Price)
                                  .FirstOrDefault())
                           .FirstOrDefaultAsync();
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
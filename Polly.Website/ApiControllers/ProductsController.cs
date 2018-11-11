using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Polly.Data;

namespace Polly.Website.Controllers
{
    [RoutePrefix("api/products")]
    public class ProductsController : ApiController
    {
        private PollyDbContext db = new PollyDbContext();

        [Route("{productId}/{currentPrice}")]
        public async Task<decimal> GetLastProductPrice(string productId, decimal currentPrice)
        {
            return await (from product in db.Product
                          where product.UniqueIdentifier == productId
                          select (from price in product.PriceHistory
                                  where price.Price != currentPrice
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
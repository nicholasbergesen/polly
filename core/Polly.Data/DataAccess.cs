using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System.Collections.Concurrent;
using System.Text.Json.Nodes;

namespace Polly.Data
{
    public class ProductDownload
    {
        public long Id { get; set; }
        public string Url { get; set; }
        public decimal Price { get; set; }
        public long PriceId { get; set; }
    }

    public class ProductIdAndPrice
    {
        public string UniqueIdentifier { get; set; }
        public decimal SellingPrice { get; set; }
        public decimal Discount { get; set; }
        public string ImageSrc { get; set; }
        public string Title { get; set; }
        public string TakealotLink { get; set; }
        public string PriceBoarLink { get; set; }
    }

    public interface IDataAccess
    {
        Task LogError(Exception exception);
        Task LogError(string message);
        Task<List<Website>> GetWebsites();
        Task<IList<DownloadQueueRepositoryItem>> GetRefreshItems(int skip, int take);
        Task UpdateLastChecked(long productId, DateTime date);
        Task SaveAsync(PriceHistory priceHistory);
        Task<IList<ProductDownload>> GetRefreshItemsAsync();
        Task<long> LastId();
        Task<int> ProductCount();
        Task<List<Product>> GetNextProduct(long fromId);
        Task UpdateDescription(Product product);
    }

    public class DataAccess : IDataAccess
    {
        private readonly IDbContextFactory<PollyDbContext> _contextFactory;
        public DataAccess(IDbContextFactory<PollyDbContext> contextFactory)
        {
            _contextFactory = contextFactory ?? throw new ArgumentNullException($"{nameof(contextFactory)} is null");
        }

        public async Task<List<Website>> GetWebsites()
        {
            using PollyDbContext context = await _contextFactory.CreateDbContextAsync();
            return await context.Website.ToListAsync();
        }

        public async Task<Website> GetWebsiteById(int id)
        {
            using PollyDbContext context = await _contextFactory.CreateDbContextAsync();
            return await context.Website.FindAsync(id);
        }

        public async Task SaveAsync(List<PriceHistory> priceHistories)
        {
            using PollyDbContext context = await _contextFactory.CreateDbContextAsync();
            foreach (var priceHistory in priceHistories)
            {
                if (priceHistory.Id == default(long))
                    context.PriceHistory.Add(priceHistory);
                else
                    context.Entry(priceHistory).State = EntityState.Modified;
            }

            await context.SaveChangesAsync();
        }

        public async Task SaveAsync(Product product)
        {
            using PollyDbContext context = await _contextFactory.CreateDbContextAsync();
            context.Database.SetCommandTimeout(TimeSpan.FromSeconds(60));
            if (product.Id == default)
                context.Product.Add(product);
            else
                context.Entry(product).State = EntityState.Modified;

            await context.SaveChangesAsync();
        }

        public async Task UpdateDescription(Product product)
        {
            using PollyDbContext context = await _contextFactory.CreateDbContextAsync();
            context.Product.Attach(product);
            context.Entry(product).Property(x => x.Description).IsModified = true;
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ProductIdAndPrice>> GetTopDiscountProducts(IEnumerable<ProductIdAndPrice> productIdandPrice)
        {
            var productIds = productIdandPrice.Select(x => x.UniqueIdentifier);
            using PollyDbContext context = await _contextFactory.CreateDbContextAsync();

            context.Database.SetCommandTimeout(TimeSpan.FromSeconds(300));
            var products = context.Product.Include(x => x.PriceHistory).Where(x => productIds.Contains(x.UniqueIdentifier));
            foreach (var prod in products)
            {
                var currentPrice = productIdandPrice.FirstOrDefault(x => x.UniqueIdentifier == prod.UniqueIdentifier);
                var latestPrice = prod.PriceHistory.Where(x => x.Price != currentPrice.SellingPrice).OrderByDescending(x => x.TimeStamp).FirstOrDefault();
                if (latestPrice == null || latestPrice.Price == currentPrice.SellingPrice)
                    continue;
                else
                {
                    currentPrice.Discount = (latestPrice.Price - currentPrice.SellingPrice) / latestPrice.Price * 100;
                    currentPrice.ImageSrc = prod.Image;
                    currentPrice.Title = prod.Title;
                    currentPrice.PriceBoarLink = $"/Home/Details/{prod.Id}";
                    currentPrice.TakealotLink = prod.Url;
                    context.PriceHistory.Add(new PriceHistory(latestPrice, currentPrice.SellingPrice));
                }
            }
            await context.SaveChangesAsync();

            return productIdandPrice.OrderByDescending(x => x.Discount).Take(50);
        }



        public async Task SaveAsync(PriceHistory priceHistory)
        {
            using PollyDbContext context = await _contextFactory.CreateDbContextAsync();
            context.Database.SetCommandTimeout(TimeSpan.FromSeconds(60));
            if (priceHistory.Id == default(long))
                context.PriceHistory.Add(priceHistory);
            else
                context.Entry(priceHistory).State = EntityState.Modified;

            await context.SaveChangesAsync();
        }

        public async Task<Product> FetchProductOrDefault(string uniqueIdentifier)
        {
            using PollyDbContext context = await _contextFactory.CreateDbContextAsync();
            context.Database.SetCommandTimeout(TimeSpan.FromSeconds(60));
            return await context.Product
                .Include(x => x.PriceHistory)
                .FirstOrDefaultAsync(x => x.UniqueIdentifier == uniqueIdentifier);
        }

        public async Task<Product> FetchProductOrDefault(long productId)
        {
            using PollyDbContext context = await _contextFactory.CreateDbContextAsync();
            return await context.Product
                .Include(x => x.PriceHistory)
                .FirstAsync(x => x.Id == productId);
        }

        public async Task UpdateLastChecked(long productId, DateTime date)
        {
            var product = new Product() { Id = productId, LastChecked = date };
            using PollyDbContext context = await _contextFactory.CreateDbContextAsync();
            context.Product.Attach(product);
            context.Entry(product).Property(x => x.LastChecked).IsModified = true;
            await context.SaveChangesAsync();
        }

        public async Task<IList<ProductDownload>> GetRefreshItemsAsync()
        {
            using PollyDbContext context = await _contextFactory.CreateDbContextAsync();
            var twoWeeksAgo = DateTime.Now.AddDays(-14);
            return await context.Product
                .Where(x => x.LastChecked < twoWeeksAgo)
                .OrderByDescending(x => x.LastChecked)
                .Select(x => new ProductDownload()
                {
                    Id = x.Id,
                    Url = x.Url,
                    Price = x.PriceHistory.OrderByDescending(y => y.Id).FirstOrDefault().Price,
                    PriceId = x.PriceHistory.OrderByDescending(y => y.Id).FirstOrDefault().Id
                })
                .Take(1000)
                .ToListAsync();
        }

        public async Task<IList<DownloadQueueRepositoryItem>> GetRefreshItems(int skip, int take)
        {
            using PollyDbContext context = await _contextFactory.CreateDbContextAsync();
            var twoWeeksAgo = DateTime.Now.AddDays(-14);
            return await context.Product
                .Where(x => x.LastChecked < twoWeeksAgo)
                .OrderByDescending(x => x.LastChecked)
                .Select(x => new DownloadQueueRepositoryItem()
                {
                    websiteId = 1,
                    DownloadLink = "https://api.takealot.com/rest/v-1-11-0/product-details/" + x.UniqueIdentifier + "?platform=desktop"
                })
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }

        public async Task<long> LastId()
        {
            using PollyDbContext context = await _contextFactory.CreateDbContextAsync();
            return await context.Product.MaxAsync(x => x.Id);
        }

        public async Task<int> ProductCount()
        {
            using PollyDbContext context = await _contextFactory.CreateDbContextAsync();
            return await context.Product.Where(x => x.Description != null).CountAsync();
        }

        public async Task<List<Product>> GetNextProduct(long fromId)
        {
            using PollyDbContext context = await _contextFactory.CreateDbContextAsync();
            return await context.Product
                .OrderBy(x => x.Id)
                .Where(x => x.Description != null && x.Id > fromId)
                .Take(2000)
                .ToListAsync();
        }

        public async Task<Product> GetProductById(long productId)
        {
            using PollyDbContext context = await _contextFactory.CreateDbContextAsync();
            var prod = await context.Product.FindAsync(productId);
            //await context.Entry(prod).Reference(p => p.PriceHistory).LoadAsync();
            return prod;
        }

        public async Task<ConcurrentQueue<long>> GetDownloadQueueProductIds()
        {
            var today = DateTime.Today;
            using PollyDbContext context = await _contextFactory.CreateDbContextAsync();
            return new ConcurrentQueue<long>(from product in context.Product
                                                where product.LastChecked < today
                                                orderby product.LastChecked
                                                select product.Id);
        }

        public async Task<PriceHistory> FetchProductLastPrice(long productId)
        {
            using PollyDbContext context = await _contextFactory.CreateDbContextAsync();
            context.Database.SetCommandTimeout(TimeSpan.FromSeconds(300));
            return await (from priceHistory in context.PriceHistory
                            where priceHistory.ProductId == productId
                            orderby priceHistory.Id descending
                            select priceHistory).FirstOrDefaultAsync();
        }

        public async Task SaveAsync(Website website)
        {
            using PollyDbContext context = await _contextFactory.CreateDbContextAsync();
            if (website.Id == default(long))
                context.Website.Add(website);
            else
                context.Entry(website).State = EntityState.Modified;

            await context.SaveChangesAsync();
        }

        public async Task LogError(Exception exception)
        {
            using PollyDbContext context = await _contextFactory.CreateDbContextAsync();
            var error = new Error(exception);
                context.Error.Add(error);
                await context.SaveChangesAsync();
        }

        public async Task LogError(string message)
        {
            using PollyDbContext context = await _contextFactory.CreateDbContextAsync();
            var error = new Error() { Message = message };
                context.Error.Add(error);
                await context.SaveChangesAsync();
        }
    }
}

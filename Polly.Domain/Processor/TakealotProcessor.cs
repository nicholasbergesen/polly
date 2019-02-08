using Newtonsoft.Json;
using Polly.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Polly.Domain
{
    public class TakealotProcessor : ITakealotProcessor
    {
        IPriceHistoryRepository _priceHistoryRepository;
        IProductRepository _productRepository;
        ICategoryRepository _categoryRepository;
        IProductCategoryRepository _productCategoryRepository;

        public TakealotProcessor(IProductRepository productRepository,
            IPriceHistoryRepository priceHistoryRepository,
            ICategoryRepository categoryRepository,
            IProductCategoryRepository productCategoryRepository)
        {
            _productRepository = productRepository;
            _priceHistoryRepository = priceHistoryRepository;
            _productCategoryRepository = productCategoryRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task HandleResultStringAsync(string downloadResult)
        {
            var takealotObject = JsonConvert.DeserializeObject<TakealotJson>(downloadResult);

            if (!takealotObject.event_data.documents.product.purchase_price.HasValue)
                return;

            var uniqueIdentifier = takealotObject.data_layer.prodid;
            decimal price = takealotObject.event_data.documents.product.purchase_price.Value;
            decimal? originalPrice = takealotObject.event_data.documents.product.original_price;
            if (price >= originalPrice)//prevent bad data
                originalPrice = null;

            var product = await _productRepository.FetchByUniqueIdAsync(uniqueIdentifier);
            bool isNew = product == null;
            HashSet<int> categoryIds = new HashSet<int>();

            if (isNew)
            {
                product = new Data.Product();
                product.UniqueIdentifier = uniqueIdentifier;
                product.Breadcrumb = takealotObject.breadcrumbs?.items.Select(x => x.name).Aggregate((i, j) => i + "," + j);
                product.Title = takealotObject.title;
                product.Description = takealotObject.description?.html;
                if (takealotObject.gallery.images.Any())
                    product.Image = takealotObject.gallery.images[0].Replace("{size}", "pdpxl");
                product.Url = takealotObject.desktop_href;
                product.Category = takealotObject.data_layer.categoryname?.Select(x => x).Aggregate((i, j) => i + "," + j);
                if (product.Category != null)
                {
                    var categories = product.Category.Split(',');
                    foreach (string description in categories)
                    {
                        if (!_categoryRepository.TryGet(description, out Category category))
                            category = await _categoryRepository.Create(description);

                        categoryIds.Add(category.Id);
                    }
                }
            }

            product.LastChecked = takealotObject.meta.date_retrieved;

            await _productRepository.SaveAsync(product);

            if (isNew)
            {
                await _priceHistoryRepository.SaveAsync(new PriceHistory(null, price, originalPrice) { ProductId = product.Id });
                await _productCategoryRepository.SaveAsync(categoryIds.Select(x => {
                    return new ProductCategory() { CategoryId = x, ProductId = product.Id };
                }));
                }
            else
            {
                var lastPrice = await _priceHistoryRepository.FetchLastPriceForProductId(product.Id);
                if (lastPrice.Price == price)
                    return;
                await _priceHistoryRepository.SaveAsync(new PriceHistory(lastPrice, price, originalPrice) { ProductId = product.Id });
            }
        }
    }
}

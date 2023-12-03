using Polly.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Polly.Domain
{
    public class TakelaotMapper : JsonMapperBase<TakealotJson>, ITakealotMapper
    {
        IPriceHistoryRepository _priceHistoryRepository;
        IProductRepository _productRepository;
        ICategoryRepository _categoryRepository;
        IProductCategoryRepository _productCategoryRepository;

        public TakelaotMapper(IPriceHistoryRepository priceHistoryRepository, IProductRepository productRepository, ICategoryRepository categoryRepository, IProductCategoryRepository productCategoryRepository)
        {
            _priceHistoryRepository = priceHistoryRepository;
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _productCategoryRepository = productCategoryRepository;

        }

        public async Task<Data.Product> MapAndSaveStringAsync(string json)
        {
            if (TryDeserialize(json, out TakealotJson takealotJson))
                return await MapAndSaveJsonAsync(takealotJson);

            return default;
        }

        public async Task<Data.Product> MapAndSaveJsonAsync(TakealotJson json)
        {
            if (IsValid(json))
                return await MapInternal(json);

            return default;
        }

        protected override async Task<Data.Product> MapInternal(TakealotJson takealotObject)
        {
            if (!takealotObject.event_data.documents.product.purchase_price.HasValue)
                return null;

            var uniqueIdentifier = takealotObject.data_layer.prodid;
            decimal price = takealotObject.event_data.documents.product.purchase_price.Value;
            decimal? originalPrice = takealotObject.event_data.documents.product.original_price;
            if (price >= originalPrice)//prevent bad data
                originalPrice = null;

            var product = await _productRepository.FetchFullProductByUniqueIdAsync(uniqueIdentifier);
            bool isNew = product == null;
            HashSet<int> categoryIds = new HashSet<int>();

            bool remapPropertyColumns = isNew || string.IsNullOrWhiteSpace(product.Title);

            if (isNew)
                product = new Data.Product();

            if (remapPropertyColumns)
            {
                product.UniqueIdentifier = uniqueIdentifier;
                product.Breadcrumb = takealotObject.breadcrumbs?.items.Select(x => x.name).Aggregate((i, j) => i + "," + j);
                product.Title = takealotObject.title;
                product.Description = takealotObject.description?.html;
                if (takealotObject.gallery.images.Any())
                    product.Image = takealotObject.gallery.images[0].Replace("{size}", "pdpxl");
                product.Url = takealotObject.desktop_href;
                product.Category = takealotObject.data_layer.categoryname?.Select(x => x).Aggregate((i, j) => i + "," + j);
                if (!string.IsNullOrEmpty(product.Category))
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

            if (isNew)
            {
                await _productRepository.SaveAsync(product);//save first to get productId
                await _priceHistoryRepository.SaveAsync(new PriceHistory(product.Id, price, originalPrice));
                await _productCategoryRepository.SaveAsync(categoryIds.Select(x => new ProductCategory() { CategoryId = x, ProductId = product.Id }));
            }
            else
            {
                var lastPrice = await _priceHistoryRepository.FetchLastPriceForProductId(product.Id);
                if (price != lastPrice?.Price)
                    await _priceHistoryRepository.SaveAsync(new PriceHistory(lastPrice, price, originalPrice) { ProductId = product.Id });

                //temporaroty until all products have categories
                if (!await _productCategoryRepository.HasCategories(product.Id))
                    await _productCategoryRepository.SaveAsync(categoryIds.Select(x => new ProductCategory() { CategoryId = x, ProductId = product.Id }));

                await _productRepository.SaveAsync(product);
            }

            return product;
        }

        protected override bool IsValid(TakealotJson takaleotDTO)
        {
            return takaleotDTO.event_data.documents.product.purchase_price.HasValue;
        }
    }
}

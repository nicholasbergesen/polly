using Polly.Data;
using System;
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

        public async Task<Data.Product> MapAndSaveAsync(string json)
        {
            if (TryDeserialize(json, out TakealotJson takealotJson) && IsValid(takealotJson))
            {
                var product = await MapInternal(takealotJson);

                await _productRepository.SaveAsync(product);

                return product;
            }
            else
                return default;
        }

        public async Task<Data.Product> MapAndSaveFullAsync(TakealotJson json)
        {
            if (IsValid(json))
            {
                var product = await MapInternal(json);

                await _productRepository.SaveAsync(product);

                return product;
            }
            else
                return default;
        }

        public async Task<Data.Product> MapAndSavePriceAsync(TakealotJson json)
        {
            if (IsValid(json))
            {
                var product = await MapInternal(json);

                await _productRepository.SaveAsync(product);

                return product;
            }
            else
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
                await _productCategoryRepository.SaveAsync(categoryIds.Select(x => new ProductCategory() { CategoryId = x, ProductId = product.Id }));
            }
            else
            {
                var lastPrice = await _priceHistoryRepository.FetchLastPriceForProductId(product.Id);
                if (price != lastPrice?.Price)
                    await _priceHistoryRepository.SaveAsync(new PriceHistory(lastPrice, price, originalPrice) { ProductId = product.Id });
                else
                    return null;

                //temporaroty until all products have categories
                if(!await _productCategoryRepository.HasCategories(product.Id))
                    await _productCategoryRepository.SaveAsync(categoryIds.Select(x => new ProductCategory() { CategoryId = x, ProductId = product.Id }));
            }
        }

        protected override bool IsValid(TakealotJson takaleotDTO)
        {
            return takaleotDTO.event_data.documents.product.purchase_price.HasValue;
        }
    }
}

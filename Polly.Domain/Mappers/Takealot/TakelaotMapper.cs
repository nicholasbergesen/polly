using Polly.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Polly.Domain
{
    public class TakelaotMapper : JsonMapperBase<TakealotJson>, ITakealotMapper
    {
        IPriceHistoryRepository _priceHistoryRepository;
        IProductRepository _productRepository;

        public TakelaotMapper(IPriceHistoryRepository priceHistoryRepository, IProductRepository productRepository)
        {
            _priceHistoryRepository = priceHistoryRepository;
            _productRepository = productRepository;
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

        protected override async Task<Data.Product> MapInternal(TakealotJson jsonObject)
        {
            bool hasPurchasePrice = !jsonObject.event_data.documents.product.purchase_price.HasValue;
            if (hasPurchasePrice)
                return null;

            decimal price = jsonObject.event_data.documents.product.purchase_price.Value;
            decimal? originalPrice = jsonObject.event_data.documents.product.original_price;

            if (price >= originalPrice)//prevent bad data
                originalPrice = null;

            Data.Product product = await _productRepository.FetchFullProductByUniqueIdAsync(jsonObject.data_layer.prodid);


            if (product != null)
            {
                var lastPrice = await _priceHistoryRepository.FetchLastPriceForProductId(product.Id);
                if (lastPrice.Price != price)
                {
                    product.PriceHistory.Add(new PriceHistory(lastPrice, price, originalPrice) { ProductId = product.Id });
                    return product;
                }
            }
            else//new product
            {
                product = new Data.Product()
                {
                    UniqueIdentifier = jsonObject.data_layer.prodid
                };
                product.PriceHistory.Add(new PriceHistory(null, price, originalPrice) { ProductId = product.Id });

                product.Breadcrumb = jsonObject.breadcrumbs?.items.Select(x => x.name).Aggregate((i, j) => i + "," + j);
                product.Title = jsonObject.title;
                product.Description = jsonObject.description?.html;
                product.Category = jsonObject.data_layer.categoryname?.Select(x => x).Aggregate((i, j) => i + "," + j);
                if (jsonObject.gallery.images.Any())
                    product.Image = jsonObject.gallery.images[0].Replace("{size}", "pdpxl");
                product.Url = jsonObject.desktop_href;
            }

            product.LastChecked = jsonObject.meta.date_retrieved;
            return product;
        }

        protected override bool IsValid(TakealotJson takaleotDTO)
        {
            return takaleotDTO.event_data.documents.product.purchase_price.HasValue;
        }
    }
}

using Polly.Data;

namespace Polly.Domain
{
    public class TakelaotMapper : MapperBase, ITakealotMapper
    {
        IDownloadQueueRepository _downloadQueueRepository;
        IPriceHistoryRepository _priceHistoryRepository;
        IProductRepository _productRepository;

        public TakelaotMapper(Ilogger logger)
            :base(logger)
        {
        }

        public bool IsValid(TakealotJson takaleotDTO)
        {
            return takaleotDTO.event_data.documents.product.purchase_price.HasValue;
        }

        public void MapToProduct(TakealotJson takealotDTO, Data.Product existingProduct)
        {
            existingProduct.UniqueIdentifier = takealotDTO.data_layer.prodid;
            //existingProduct.Breadcrumb = takealotDTO.breadcrumbs?.items.Select(x => x.name).Aggregate((i, j) => i + "," + j);
            existingProduct.Title = takealotDTO.title;
            existingProduct.Description = takealotDTO.description?.html;
            //existingProduct.Category = takealotDTO.data_layer.categoryname?.Select(x => x).Aggregate((i, j) => i + "," + j);
            //if (takealotDTO.gallery.images.Any())
            //    existingProduct.Image = takealotDTO.gallery.images[0].Replace("{size}", "pdpxl");
            existingProduct.Url = takealotDTO.desktop_href;
            existingProduct.LastChecked = takealotDTO.meta.date_retrieved;

            MapToPriceHistory(takealotDTO, new PriceHistory());

            //_priceHistoryRepository.Save(new PriceHistory(lastPrice, price, originalPrice) { ProductId = product.Id }));
        }

        public void MapToPriceHistory(TakealotJson takealotDTO, PriceHistory priceHistory)
        {
            //if (existingProduct != null)
            //{
            //    var lastPrice = _priceHistoryRepository.FetchProductLastPrice(existingProduct.Id);
            //    if (lastPrice.Price == price)
            //        return;
            //    _priceHistoryRepository.Save(new PriceHistory(lastPrice, price, originalPrice) { ProductId = product.Id }));
            //    //return;
            //}
        }
    }
}

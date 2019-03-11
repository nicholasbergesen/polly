using SimpleInjector;

namespace Polly.Data
{
    public static class RegisterDI
    {
        public static void Register(Container container)
        {
            container.Register<IProductRepository, ProductRepository>();
            //container.Register<IDownloadQueueRepository, DownloadQueueRepository>();
            container.Register<IPriceHistoryRepository, PriceHistoryRepository>();
            container.Register<ICategoryRepository, CategoryRepository>();
            container.Register<IProductCategoryRepository, ProductCategoryRepository>();
        }
    }
}

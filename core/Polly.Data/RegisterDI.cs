using SimpleInjector;

namespace Polly.Data
{
    public static class RegisterDI
    {
        public static void Register(Container container, ScopedLifestyle lifestyle = null)
        {
            if (lifestyle == default)
            {
                container.Register<IProductRepository, ProductRepository>();
                //container.Register<IDownloadQueueRepository, DownloadQueueRepository>();
                container.Register<IDownloadQueueRepository, DownloadQueueFileRepository>();
                container.Register<IPriceHistoryRepository, PriceHistoryRepository>();
                container.Register<ICategoryRepository, CategoryRepository>();
                container.Register<IProductCategoryRepository, ProductCategoryRepository>();

            }
            else
            {
                container.Register<IProductRepository, ProductRepository>(lifestyle);
                //container.Register<IDownloadQueueRepository, DownloadQueueRepository>(lifestyle);
                container.Register<IDownloadQueueRepository, DownloadQueueFileRepository>(lifestyle);
                container.Register<IPriceHistoryRepository, PriceHistoryRepository>(lifestyle);
                container.Register<ICategoryRepository, CategoryRepository>(lifestyle);
                container.Register<IProductCategoryRepository, ProductCategoryRepository>(lifestyle);
            }
        }
    }
}

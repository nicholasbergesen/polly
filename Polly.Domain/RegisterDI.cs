using SimpleInjector;

namespace Polly.Domain
{
    public static class RegisterDI
    {
        public static void Register(Container container, ScopedLifestyle lifestyle = null)
        {
            if (lifestyle == default)
            {
                container.Register<IDownloader, TakealotDownloader>();
                container.Register<ITakealotMapper, TakelaotMapper>();
                container.Register<ILootMapper, LootMapper>();
            }
            else
            {
                container.Register<IDownloader, TakealotDownloader>(lifestyle);
                container.Register<ITakealotMapper, TakelaotMapper>(lifestyle);
                container.Register<ILootMapper, LootMapper>(lifestyle);
            }
        }
    }
}

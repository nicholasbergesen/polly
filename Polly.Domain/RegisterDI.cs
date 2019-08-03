using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Text;

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
                container.Register<ITakealotProcessor, TakealotProcessor>();
                container.Register<ITakealotScheduler, TakelaotScheduler>();
                container.Register<ILootScheduler, LootScheduler>();
            }
            else
            {
                container.Register<IDownloader, TakealotDownloader>(lifestyle);
                container.Register<ITakealotMapper, TakelaotMapper>(lifestyle);
                container.Register<ITakealotProcessor, TakealotProcessor>(lifestyle);
                container.Register<ITakealotScheduler, TakelaotScheduler>(lifestyle);
                container.Register<ILootScheduler, LootScheduler>(lifestyle);
            }
        }
    }
}

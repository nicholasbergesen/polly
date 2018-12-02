using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Text;

namespace Polly.Domain
{
    public static class RegisterDI
    {
        public static void Register(Container container)
        {
            container.Register<IDownloader, TakealotDownloader>();
            container.Register<ITakealotMapper, TakelaotMapper>();
            container.Register<ITakealotProcessor, TakealotProcessor>();
        }
    }
}

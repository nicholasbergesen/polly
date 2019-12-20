﻿using SimpleInjector;
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
            }
            else
            {
                container.Register<IDownloader, TakealotDownloader>(lifestyle);
                container.Register<ITakealotMapper, TakelaotMapper>(lifestyle);
            }
        }
    }
}

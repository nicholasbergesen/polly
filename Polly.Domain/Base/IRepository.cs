using System;
using System.Collections.Generic;
using System.Text;

namespace Polly.Domain
{
    interface IRepository<T>
    {
        void Save(T domainObject);
    }
}

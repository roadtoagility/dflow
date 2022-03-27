using System;
using System.Collections.Generic;

namespace DFlow.Configuration
{
    public interface IDependencyResolver
    {
        IEnumerable<object> Resolve(Type service);
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using DFlow.Interfaces;

namespace DFlow.Configuration
{
    public interface IDependencyResolver
    {
        IEnumerable<object> Resolve(Type service);
    }
}
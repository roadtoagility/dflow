using System;

namespace DFlow.Configuration
{
    public interface IDependencyResolver
    {
        object Resolve(Type service);
    }
}
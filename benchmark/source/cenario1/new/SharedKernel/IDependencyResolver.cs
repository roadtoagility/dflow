using System;

namespace SharedKernel
{
    public interface IDependencyResolver
    {
        object Resolve(Type service);
    }
}
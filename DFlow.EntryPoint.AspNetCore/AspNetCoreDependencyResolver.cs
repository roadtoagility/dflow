using System;
using DFlow.Configuration;

namespace DFlow.EntryPoint.AspNetCore
{
    public class AspNetCoreDependencyResolver : IDependencyResolver
    {
        private readonly IServiceProvider _provider;
        
        public AspNetCoreDependencyResolver(IServiceProvider provider)
        {
            _provider = provider;
        }

        public object Resolve(Type service)
        {
            return _provider.GetService(service);
        }
    }
}
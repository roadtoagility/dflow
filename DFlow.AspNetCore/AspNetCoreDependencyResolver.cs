using System;
using System.Collections.Generic;
using DFlow.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DFlow.AspNetCore
{
    public class AspNetCoreDependencyResolver : IDependencyResolver
    {
        private readonly IServiceProvider _provider;
        
        public AspNetCoreDependencyResolver(IServiceProvider provider)
        {
            _provider = provider;
        }

        public IEnumerable<object> Resolve(Type service)
        {
            return _provider.GetServices(service);
        }
    }
}
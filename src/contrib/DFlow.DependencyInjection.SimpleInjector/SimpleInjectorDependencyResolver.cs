using System;
using System.Collections.Generic;
using DFlow.Configuration;
using SimpleInjector;

namespace DFlow.DependencyInjection
{
    public class SimpleInjectorDependencyResolver : IDependencyResolver
    {
        private readonly Container _container;

        public SimpleInjectorDependencyResolver(Container container)
        {
            _container = container;
        }

        public IEnumerable<object> Resolve(Type service)
        {
            return _container.GetAllInstances(service);
        }
    }
}
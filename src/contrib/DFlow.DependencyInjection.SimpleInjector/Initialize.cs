using System;
using DFlow.Base;
using DFlow.Configuration;
using DFlow.Interfaces;
using Microsoft.AspNetCore.Builder;
using SimpleInjector;

namespace DFlow.DependencyInjection
{
    public static class Initialize
    {
        public static void InitDlow(
            this IApplicationBuilder builder, Container provider)
        {
            var container = new SimpleInjectorDependencyResolver(provider);
            provider.RegisterInstance<IDependencyResolver>(container);
            provider.Register<IEventStore<Guid>, EventStore>();
        }
    }
}
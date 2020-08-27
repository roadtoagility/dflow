using System;
using DFlow.Configuration;
using DFlow.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
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
        }
    }
}
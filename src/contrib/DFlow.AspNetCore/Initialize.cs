using System;
using DFlow.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace DFlow.AspNetCore
{
    public static class Initialize
    {
        public static void InitDlow(
            this IApplicationBuilder builder, IServiceCollection collection)
        {
            var serviceProvider = builder.ApplicationServices;
            var container = new AspNetCoreDependencyResolver(serviceProvider);
            collection.AddScoped<IDependencyResolver, AspNetCoreDependencyResolver>();
        }
    }
}
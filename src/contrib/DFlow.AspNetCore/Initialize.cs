using System;
using DFlow.Base;
using DFlow.Configuration;
using DFlow.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace DFlow.AspNetCore
{
    public static class Initialize
    {
        public static void InitDlow(
            this IApplicationBuilder builder, IServiceCollection collection)
        {
            collection.AddScoped<IDependencyResolver, AspNetCoreDependencyResolver>();
            collection.AddScoped<IEventStore<Guid>, EventStore>();
        }
    }
}
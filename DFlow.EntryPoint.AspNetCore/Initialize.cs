using System;
using DFlow.Configuration.Startup;
using DFlow.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace DFlow.EntryPoint.AspNetCore
{
    public static class Initialize
    {
        public static void InitDlow(
            this IApplicationBuilder builder, IServiceProvider provider)
        {
            RegisterSubscribers(provider);
            builder.UseMiddleware<MonitorMiddleware>();
        }

        private static void RegisterSubscribers(IServiceProvider provider)
        {
            var eventBus = provider.GetService<IEventBus>();
            var resolver = new AspNetCoreDependencyResolver(provider);
            SubscriberFactory.Instance.RegisterSubscribers(eventBus, resolver);
        }
    }
}
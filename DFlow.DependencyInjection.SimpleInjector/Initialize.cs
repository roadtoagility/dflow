using System;
using DFlow.Configuration.Startup;
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
            RegisterSubscribers(provider);
            // builder.UseMiddleware<MonitorMiddleware>();
        }

        private static void RegisterSubscribers(Container provider)
        {
            var eventBus = provider.GetInstance<IEventBus>();
            var resolver = new SimpleInjectorDependencyResolver(provider);
            SubscriberFactory.Instance.RegisterSubscribers(eventBus, resolver);
        }
    }
}
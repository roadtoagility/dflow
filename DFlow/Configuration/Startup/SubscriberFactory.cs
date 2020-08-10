using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DFlow.Interfaces;

namespace DFlow.Configuration.Startup
{
    // public static class SubscriberFactory
    // {
    //     public static void RegisterSubscribers(IEventBus eventBus)
    //     {
    //         
    //     }
    // }
    
    public sealed class SubscriberFactory
    {
        private SubscriberFactory()
        {
            
        }
        
        private static  readonly Lazy<SubscriberFactory> lazy = new Lazy<SubscriberFactory>(() => new SubscriberFactory());
        
        public static SubscriberFactory Instance
        {
            get { return lazy.Value; }
        }

        public void RegisterSubscribers(IEventBus eventBus, IDependencyResolver provider)
        {
            var subscribers = from x 
                    in AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(s => s.GetTypes())
                let y = x.BaseType
                where !x.IsAbstract && !x.IsInterface &&
                      y != null && y.IsGenericType &&
                      y.GetGenericTypeDefinition() == typeof(ISubscriber<>)
                select x;

            foreach (var subType in subscribers)
            {
                var constructor = subType.GetConstructors();
                var parameters = constructor[0].GetParameters();
                var parametersConcrete = new List<object>();

                foreach (var parameter in parameters)
                {
                    parametersConcrete.Add(provider.Resolve(parameter.GetType()));
                }

                var concreteSubscribe = Activator.CreateInstance(subType, parametersConcrete.ToArray());
                
                eventBus.Subscribe(concreteSubscribe);
            }
        }
    }
}
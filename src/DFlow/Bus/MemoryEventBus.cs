using DFlow.Configuration;
using DFlow.Interfaces;

namespace DFlow.Bus
{
    public class MemoryEventBus : IEventBus
    {
        private readonly IDependencyResolver _resolver;

        public MemoryEventBus(IDependencyResolver resolver)
        {
            _resolver = resolver;
        }

        public void Publish(params IEvent[] events)
        {
            foreach (var @event in events)
            {
                var subscribers = _resolver.Resolve(@event.GetType());

                foreach (var subscriber in subscribers) ((dynamic)subscriber).Update((dynamic)@event);
            }
        }
    }
}
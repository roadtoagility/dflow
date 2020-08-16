using System;
using DFlow.Bus;
using DFlow.Example;
using DFlow.Example.Events;
using DFlow.Example.Views;
using DFlow.Interfaces;
using Xunit;

namespace DFlow.Tests
{
    public class EventBusTests
    {
        private IEventBus _eventBus;
        private MemoryResolver _resolver;
        
        public EventBusTests()
        {
            _resolver = new MemoryResolver();
            _eventBus = new MemoryEventBus(_resolver);
        }
        
        [Fact]
        public void ShouldSubscribeAndNotified()
        {
            var view = new ProductView();
            
            //DI resolver
            _resolver.Register<ProductCreated>(view);
            
            var productId = Guid.NewGuid();
            _eventBus.Publish(new ProductCreated(productId, "name", "description"));
            
            Assert.True(view.Products.Count == 1);
            Assert.Contains(view.Products, x => x.Id == productId);
        }

        [Fact]
        public void ShoudUnsubscribe()
        {
            var view = new ProductView();
            
            _resolver.Register<ProductCreated>(view);
            var productId = Guid.NewGuid();
            _eventBus.Publish(new ProductCreated(productId, "name", "description"));
            _resolver.Unregister<ProductCreated>(view);
            _eventBus.Publish(new ProductCreated(Guid.NewGuid(), "name", "description"));
            
            Assert.True(view.Products.Count == 1);
            Assert.Contains(view.Products, x => x.Id == productId);
        }
    }
}
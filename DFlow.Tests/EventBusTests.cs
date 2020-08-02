using System;
using DFlow.Bus;
using DFlow.Example.Events;
using DFlow.Example.Views;
using DFlow.Interfaces;
using Xunit;

namespace DFlow.Tests
{
    public class EventBusTests
    {
        private IEventBus _eventBus;
        
        public EventBusTests()
        {
            _eventBus = new MemoryEventBus();
        }
        
        [Fact]
        public void ShouldSubscribeAndNotified()
        {
            var view = new ProductView();
            
            _eventBus.Subscribe<ProductCreated>(view);
            var productId = Guid.NewGuid();
            _eventBus.Publish(new ProductCreated(productId, "name", "description"));
            
            Assert.True(view.Products.Count == 1);
            Assert.Contains(view.Products, x => x.Id == productId);
        }

        [Fact]
        public void ShoudUnsubscribe()
        {
            var view = new ProductView();
            
            _eventBus.Subscribe<ProductCreated>(view);
            var productId = Guid.NewGuid();
            _eventBus.Publish(new ProductCreated(productId, "name", "description"));
            _eventBus.Unsubscribe<ProductCreated>(view);
            _eventBus.Publish(new ProductCreated(Guid.NewGuid(), "name", "description"));
            
            Assert.True(view.Products.Count == 1);
            Assert.Contains(view.Products, x => x.Id == productId);
        }
    }
}
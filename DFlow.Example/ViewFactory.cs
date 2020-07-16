using System;
using System.Collections.Generic;
using System.Linq;
using DFlow.Interfaces;
using DFlow.Example.Events;
using DFlow.Example.Handlers;
using DFlow.Example.Views;

namespace DFlow.Example
{
    public class ViewFactory
    {
        private readonly IEventBus _eventBus;
        
        public ViewFactory(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public IReadModel<ProductDTO> CreateProductView()
        {
            var productView = new ProductView();
            _eventBus.Subscribe<ProductCreated>(productView);
            _eventBus.Subscribe<ProductNameChanged>(productView);
            return productView;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using DFlow.Interfaces;
using Program.Events;
using Program.Handlers;
using Program.Views;

namespace Program
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
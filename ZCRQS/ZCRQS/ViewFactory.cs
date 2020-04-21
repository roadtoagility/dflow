using System;
using System.Collections.Generic;
using System.Linq;
using Core.Shared.Interfaces;
using Program.Events;
using Program.Handlers;
using Program.Views;

namespace Program
{
    public class ViewFactory
    {
        private readonly IQueueService _queueService;
        
        public ViewFactory(IQueueService queueService)
        {
            _queueService = queueService;
        }

        public IReadModel<ProductDTO> CreateProductView()
        {
            var productView = new ProductView();
            _queueService.Subscribe<ProductCreated>(productView);
            _queueService.Subscribe<ProductNameChanged>(productView);
            return productView;
        }
    }
}
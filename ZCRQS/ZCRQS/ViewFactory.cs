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
        private readonly IReadModel<IList<ProductDTO>> _readModel;
        private readonly IQueueService _queueService;
        
        public ViewFactory(IReadModel<IList<ProductDTO>> readModel, IQueueService queueService)
        {
            _readModel = readModel;
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
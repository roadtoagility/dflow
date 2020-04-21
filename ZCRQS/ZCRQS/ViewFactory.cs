using Core.Shared.Interfaces;
using Program.Events;
using Program.Views;

namespace Program
{
    public class ViewFactory
    {
        private readonly IReadModel _readModel;
        private readonly IQueueService _queueService;
        
        public ViewFactory(IReadModel readModel, IQueueService queueService)
        {
            _readModel = readModel;
            _queueService = queueService;
        }

        public ProductView CreateProductView()
        {
            // var productsDto = _readModel.Query<ProductDTO>("products");
            //
            // var productView = new ProductView(productsDto);
            // _queueService.Subscribe<ProductCreated>(productView);
            // _queueService.Subscribe<ProductNameChanged>(productView);
            // return productView;
            return new ProductView();
        }
    }
}
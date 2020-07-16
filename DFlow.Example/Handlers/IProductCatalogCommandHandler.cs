
using DFlow.Interfaces;

namespace DFlow.Example.Handlers
{
    public interface IProductCatalogCommandHandler
    {
        void Execute(ICommand cmd);
    }
}
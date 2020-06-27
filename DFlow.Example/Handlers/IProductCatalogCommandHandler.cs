using Core.Shared;
using DFlow.Interfaces;

namespace Program.Handlers
{
    public interface IProductCatalogCommandHandler
    {
        void Execute(ICommand cmd);
    }
}
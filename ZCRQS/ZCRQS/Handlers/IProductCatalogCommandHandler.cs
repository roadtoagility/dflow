using Core.Shared;
using Core.Shared.Interfaces;

namespace Program.Handlers
{
    public interface IProductCatalogCommandHandler
    {
        void Execute(ICommand cmd);
    }
}
using DFlow.Base.Events;
using DFlow.Interfaces;

namespace DFlow.Example.Handlers
{
    public interface IProductCatalogCommandHandler
    {
        CommandEvent Execute(ICommand cmd);
    }
}
using Core.Shared;
using Core.Shared.Interfaces;

namespace Program.Handlers
{
    public interface IProductServiceCommandHandler
    {
        void Execute(ICommand cmd);
    }
}
using Core.Shared;
using Core.Shared.Interfaces;

namespace Program.Handlers
{
    public interface IOrderServiceCommandHandler
    {
        void Execute(ICommand cmd);
    }
}
using Core.Shared;

namespace Program.Handlers
{
    public interface IOrderServiceCommandHandler
    {
        void Execute(ICommand cmd);
    }
}
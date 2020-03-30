using Core.Shared;

namespace Program.Handlers
{
    public interface IProductServiceCommandHandler
    {
        void Execute(ICommand cmd);
    }
}
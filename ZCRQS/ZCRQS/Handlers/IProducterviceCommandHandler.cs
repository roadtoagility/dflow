using Core.Shared;

namespace Program.Handlers
{
    public interface IProducterviceCommandHandler
    {
        void Execute(ICommand cmd);
    }
}
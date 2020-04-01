namespace SharedKernel.Distribuited
{
    public interface IHandler
    {
        string GetName();
        object Initialize(object data);
    }
}
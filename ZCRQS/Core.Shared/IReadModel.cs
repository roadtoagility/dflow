namespace Core.Shared
{
    public interface IReadModel<T, U>
    {
        T Get(U param);
    }
}
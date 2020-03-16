namespace Core.Shared
{
    public interface IWriteRepository<T>
    {
        void OnUpdate<U>(U entity);
    }
}
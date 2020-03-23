namespace Core.Shared
{
    public abstract class RepositoryBase
    {

        protected abstract void OnUpdate<T>(T entity);

        public void Update<T>(T entity) where T:IEventAggregate
        {
            OnUpdate<T>(entity);
            
            //EventStream.Instance.Raise(entity.GetEvents());
        }
    }
}
namespace Core.Shared
{
    public interface IEventAggregate
    {
        Event[] GetEvents();
    }
}
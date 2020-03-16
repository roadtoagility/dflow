namespace Core.Shared
{
    public interface IObserverStream
    {
        void Raise(Event eventUnit);
        string[] GetEventListers();
    }
}
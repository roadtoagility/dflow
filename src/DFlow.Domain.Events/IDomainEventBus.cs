namespace DFlow.Domain.Events
{
    public interface IDomainEventBus
    {
        void Publish<TEvent>(TEvent request);

        TResult Send<TResult,TRequest>(TRequest request);
    }
}
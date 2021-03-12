using System.Collections.Immutable;
using DFlow.Domain.Aggregates;
using DFlow.Domain.BusinessObjects;
using DFlow.Domain.Events;
using DFlow.Tests.Supporting.DomainObjects.Events;

namespace DFlow.Tests.Supporting.DomainObjects
{
    public sealed class EventStreamBusinessEntityAggregateRoot:EventBasedAggregationRoot<EntityTestId>
    {
        private EventStreamBusinessEntityAggregateRoot(EventStream<EntityTestId> eventStream)
        :base(eventStream.AggregationId,eventStream.Version)
        {
            // if (eventStream.ValidationResults.IsValid)
            // {
                Apply(eventStream.Events);

                if (eventStream.IsNew())
                {
                    Raise(TestEntityAggregateAddedDomainEvent.For(eventStream));                    
                }
            // }

            // ValidationResults = eventStream.ValidationResults;
        }

        public static EventStreamBusinessEntityAggregateRoot Create(EntityTestId id, ImmutableList<IDomainEvent> events)
        {
            return new EventStreamBusinessEntityAggregateRoot(EventStream<EntityTestId>.From(id, Version.New(), events));
        }
        
        public static EventStreamBusinessEntityAggregateRoot ReconstructFrom(EventStream<EntityTestId> eventStream)
        {
            return new EventStreamBusinessEntityAggregateRoot(EventStream<EntityTestId>.From(eventStream.AggregationId, Version.Next(eventStream.Version), eventStream.Events));
        }
    }
}
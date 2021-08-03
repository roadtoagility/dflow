using System.Collections.Immutable;
using DFlow.Domain.Aggregates;
using DFlow.Domain.BusinessObjects;
using DFlow.Tests.Supporting.DomainObjects.Events;

namespace DFlow.Tests.Supporting.DomainObjects
{
    public sealed class EventStreamBusinessEntityAggregateRoot:EventBasedAggregationRoot<EntityTestId>
    {
        private EventStreamBusinessEntityAggregateRoot(EntityTestId aggregationId, Name name, Email email, VersionId version)
            :base(aggregationId,version, AggregationName.From(nameof(EventStreamBusinessEntityAggregateRoot)))
        {
            if (name.ValidationResults.IsValid && email.ValidationResults.IsValid)
            {
                var change = TestEntityAggregateAddedDomainEvent.From(aggregationId, name, email, version);
                Apply(change);
                
                // it is always new
                Raise(change);
            }
            ValidationResults = name.ValidationResults;
        }

        private EventStreamBusinessEntityAggregateRoot(EventStream<EntityTestId> eventStream)
            : base(eventStream.AggregationId, eventStream.Version,
                AggregationName.From(nameof(EventStreamBusinessEntityAggregateRoot)))
        {
            if (eventStream.ValidationResults.IsValid)
            {
                Apply(eventStream.Events);
            }

            ValidationResults = eventStream.ValidationResults;
        }

        public void UpdateName(EntityTestId aggregateId, Name name)
        {
            if (name.ValidationResults.IsValid && !AggregateId.Equals(aggregateId))
            {
                Apply(TestEntityAggregateUpdatedDomainEvent.From(AggregateId,name,Version));
            }

            ValidationResults = name.ValidationResults;
        }

        public static EventStreamBusinessEntityAggregateRoot Create(EntityTestId aggregateId, Name name, Email email)
        {
            return new EventStreamBusinessEntityAggregateRoot(aggregateId, name, email, VersionId.New());
        }
        
        public static EventStreamBusinessEntityAggregateRoot ReconstructFrom(EventStream<EntityTestId> eventStream)
        {
            return new EventStreamBusinessEntityAggregateRoot(EventStream<EntityTestId>.From(eventStream.AggregationId, 
                eventStream.Name,VersionId.Next(eventStream.Version), eventStream.Events));
        }
    }
}
using System.Collections.Generic;
using System.Collections.Immutable;
using DFlow.Domain.Events;
using DFlow.Domain.Validation;

namespace DFlow.Domain.BusinessObjects
{
    public class EventStream<TEntityId>: ValidationStatus
    {
        private EventStream(TEntityId aggregateId, Version version, IImmutableList<IDomainEvent> events)
        {
            AggregationId = aggregateId;
            Version = version;
            Events = events;
        }
        
        public TEntityId AggregationId { get; }
        public Version Version { get; }
        public IImmutableList<IDomainEvent> Events { get; }
        
        public bool IsNew() => Version.Equals(Version.New());

        public static EventStream<TEntityId> From(TEntityId aggregateId, Version version, IImmutableList<IDomainEvent> events)
        {
            var eventStream = new EventStream<TEntityId>(aggregateId,version,events);
            return eventStream;
        }

        public static EventStream<TEntityId> AppendStream(EventStream<TEntityId> stream, IImmutableList<IDomainEvent> appendEvents)
        {
            var newStream = stream.Events.AddRange(appendEvents);
            return From(stream.AggregationId, Version.Next(stream.Version), newStream);
        }
        
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return AggregationId;
            yield return Version;
        }
    }
}
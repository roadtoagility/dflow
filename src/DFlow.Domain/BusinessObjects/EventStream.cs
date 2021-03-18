using System.Collections.Generic;
using System.Collections.Immutable;
using DFlow.Domain.Events;
using DFlow.Domain.Validation;

namespace DFlow.Domain.BusinessObjects
{
    public class EventStream<TEntityId>: ValidationStatus
    {
        private EventStream(TEntityId aggregateId, AggregationName aggregationName, Version version, IImmutableList<IDomainEvent> events)
        {
            AggregationId = aggregateId;
            Name = aggregationName;
            Version = version;
            Events = events;
        }
        
        public TEntityId AggregationId { get; }
        public AggregationName Name { get; }
        public Version Version { get; }
        public IImmutableList<IDomainEvent> Events { get; }
        
        public bool IsNew() => Version.Equals(Version.New());

        public static EventStream<TEntityId> From(TEntityId aggregateId, AggregationName name, Version version, IImmutableList<IDomainEvent> events)
        {
            var eventStream = new EventStream<TEntityId>(aggregateId,name,version,events);
            return eventStream;
        }

        public static EventStream<TEntityId> AppendStream(EventStream<TEntityId> stream, IImmutableList<IDomainEvent> appendEvents)
        {
            var newStream = stream.Events.AddRange(appendEvents);
            return From(stream.AggregationId, stream.Name, Version.Next(stream.Version), newStream);
        }
        
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return AggregationId;
            yield return Name;
            yield return Version;
        }
    }
}
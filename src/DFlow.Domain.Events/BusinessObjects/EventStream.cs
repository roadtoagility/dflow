// Copyright (C) 2022  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Collections.Generic;
using System.Collections.Immutable;
using DFlow.Domain.BusinessObjects;

namespace DFlow.Domain.Events.BusinessObjects
{
    public class EventStream<TEntityId>: BaseEntity<TEntityId>
    {
        private EventStream(TEntityId aggregateId, AggregationName aggregationName
            , VersionId version, IImmutableList<IDomainEvent> events)
            : base(aggregateId, version)
        {
            AggregationId = aggregateId;
            Name = aggregationName;
            Events = events;
        }

        public TEntityId AggregationId { get; }
        public AggregationName Name { get; }
        public IImmutableList<IDomainEvent> Events { get; }
        
        public static EventStream<TEntityId> From(TEntityId aggregateId, AggregationName name, VersionId version, IImmutableList<IDomainEvent> events)
        {
            var eventStream = new EventStream<TEntityId>(aggregateId,name,version,events);
            return eventStream;
        }

        public static EventStream<TEntityId> AppendStream(EventStream<TEntityId> stream, IImmutableList<IDomainEvent> appendEvents)
        {
            var newStream = stream.Events.AddRange(appendEvents);
            return From(stream.AggregationId, stream.Name, VersionId.Next(stream.Version), newStream);
        }
        
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return AggregationId;
            yield return Name;
            yield return Version;
        }
    }
}
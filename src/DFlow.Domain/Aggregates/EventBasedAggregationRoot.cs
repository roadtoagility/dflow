// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Collections.Generic;
using System.Collections.Immutable;
using DFlow.Domain.BusinessObjects;
using DFlow.Domain.Events;
using FluentValidation.Results;

namespace DFlow.Domain.Aggregates
{
    public class EventBasedAggregationRoot<TEntityId> : IChangeSet<EventStream<TEntityId>>
    {
        private readonly TEntityId _aggregateId;
        private readonly Version _version;
        private readonly List<IDomainEvent> _changes = new List<IDomainEvent>();

        protected EventBasedAggregationRoot(TEntityId aggregateId, Version version, IImmutableList<IDomainEvent> events)
        {
            _aggregateId = aggregateId;
            _version = version;
            _changes.AddRange(events);
        }
        
        protected void Apply(IDomainEvent item)
        {
            _changes.Add(item);
        }
        
        protected void Raise(IDomainEvent @event)
        {
            _changes.Add(@event);
        }
        
        public EventStream<TEntityId> GetChange()
        {
            return EventStream<TEntityId>.From(_aggregateId,_version, _changes.ToImmutableList());
        }

        public IReadOnlyList<IDomainEvent> GetEvents()
        {
            return _changes.ToImmutableList();
        }

        public ValidationResult ValidationResults { get; protected set; }

        public EventBasedAggregationRoot<TEntityId> Create(TEntityId aggregateId, IImmutableList<IDomainEvent> events)
        {
            return new EventBasedAggregationRoot<TEntityId>(aggregateId, Version.New(), events);
        }
    }
}
// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Collections.Generic;
using System.Collections.Immutable;
using DFlow.Domain.BusinessObjects;
using DFlow.Domain.Events;
using DFlow.Domain.Validation;

namespace DFlow.Domain.Aggregates
{
    public class EventBasedAggregationRoot<TEntityId> : BaseValidation, IChangeSet<EventStream<TEntityId>>
    {
        private readonly List<IDomainEvent> _changes;
        private readonly List<IDomainEvent> _currentStream;

        protected EventBasedAggregationRoot(TEntityId aggregateId, VersionId version, AggregationName name)
        {
            Name = name;
            AggregateId = aggregateId;
            Version = version;
            _currentStream = new List<IDomainEvent>();
            _changes = new List<IDomainEvent>();
        }

        protected AggregationName Name { get; }

        protected TEntityId AggregateId { get; }

        protected VersionId Version { get; }

        public EventStream<TEntityId> GetChange()
        {
            return EventStream<TEntityId>.From(AggregateId, Name, Version, _currentStream.ToImmutableList());
        }

        public IReadOnlyList<IDomainEvent> GetEvents()
        {
            return _changes.ToImmutableList();
        }

        protected void Apply(IImmutableList<IDomainEvent> domainEvents)
        {
            foreach (var ev in domainEvents) Apply(ev);
        }

        protected void Apply(IDomainEvent domainEvent)
        {
            _currentStream.Add(domainEvent);
        }

        //Need to check tath
        protected void Raise(IDomainEvent @event)
        {
            //this isint right
            _changes.Add(@event);
        }
    }
}
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
        private readonly List<IDomainEvent> _currentStream;
        private readonly List<IDomainEvent> _changes;

        protected EventBasedAggregationRoot(TEntityId aggregateId, Version version, AggregationName name)
        {
            Name = name;
            AggregateId = aggregateId;
            Version = version;
            _currentStream = new List<IDomainEvent>();
            _changes = new List<IDomainEvent>();
        }
        
        protected void Apply(IImmutableList<IDomainEvent> domainEvents)
        {
            foreach (var ev in domainEvents)
            {
                Apply(ev);                
            }
        }
        
        protected AggregationName Name { get; }
        
        protected TEntityId AggregateId { get; }
        
        protected Version Version { get; }
        
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
        
        public EventStream<TEntityId> GetChange()
        {
            return EventStream<TEntityId>.From(AggregateId,Name ,Version,  _changes.ToImmutableList());
        }

        public IReadOnlyList<IDomainEvent> GetEvents()
        {
            return _changes.ToImmutableList();
        }

        public ValidationResult ValidationResults { get; protected set; }
    }
}
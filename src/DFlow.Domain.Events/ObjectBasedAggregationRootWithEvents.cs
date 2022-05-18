// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Collections.Generic;
using System.Collections.Immutable;
using DFlow.Domain.Aggregates;
using DFlow.Domain.BusinessObjects;

namespace DFlow.Domain.Events
{
    public abstract class ObjectBasedAggregationRootWithEvents<TChange, TEntityId>: 
        ObjectBasedAggregationRoot<TChange, TEntityId> where TChange:BaseEntity<TEntityId>
    {
        private readonly IList<IDomainEvent> _changes = new List<IDomainEvent>();
        
        protected void Raise(IDomainEvent @event)
        {
            _changes.Add(@event);
        }

        public IReadOnlyList<IDomainEvent> GetEvents()
        {
            return _changes.ToImmutableList();
        }
    }
}
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
using FluentValidation.Results;

namespace DFlow.Domain.Aggregates
{
    public abstract class BasedAggregationRoot<TAggregateId>: BaseValidation //, IChangeSet<TChange>
    {
        // protected TChange AggregateRootEntity { get; set; }
        private readonly IList<IDomainEvent> _changes = new List<IDomainEvent>();

        // protected void Apply(TChange item)
        // {
        //     AggregateRootEntity = item;
        // }

        protected BasedAggregationRoot(IEntityIdentity<TAggregateId> aggregateId, VersionId versionId)
        {
            AggregateId = aggregateId;
            Version = versionId;
        }
        
        protected void Raise(IDomainEvent @event)
        {
            _changes.Add(@event);
        }
        
        // public TChange GetChange()
        // {
        //     return AggregateRootEntity;
        // }

        public IReadOnlyList<IDomainEvent> GetEvents()
        {
            return _changes.ToImmutableList();
        }
        
        protected IEntityIdentity<TAggregateId> AggregateId { get; }
        
        protected VersionId Version { get; }

    }
}
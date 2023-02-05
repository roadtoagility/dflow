// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using DFlow.BusinessObjects;
using DFlow.Events;
using DFlow.Validation;

namespace DFlow.Aggregates
{
    public abstract class AggregateBase<TChange, TEntityId> : BaseValidation,
        IAggregate<TChange> where TChange : EntityBase<TEntityId>
    {
        protected AggregateBase(TChange root)
        {
            Root = root;
        }

        protected TChange Root { get; private set; }

        protected void Apply(TChange item)
        {
            Root = item;
        }

        protected void Raise(DomainEvent @event)
        {
            Root.RaisedEvent(@event);
        }

        public TChange GetChange()
        {
            return Root;
        }
    }
}
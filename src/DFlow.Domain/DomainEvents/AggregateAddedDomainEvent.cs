// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System;
using DFlow.Domain.BusinessObjects;
using DFlow.Domain.Events;
using Version = DFlow.Domain.BusinessObjects.Version;

namespace DFlow.Domain.DomainEvents
{
    public class AggregateAddedDomainEvent<TEntityId> : DomainEvent
    {
        protected AggregateAddedDomainEvent(EventStream<TEntityId> stream)
        :base(DateTime.Now, stream.Version)
        {
            EventStream = stream;
        }

        public EventStream<TEntityId> EventStream { get; }
    }
}
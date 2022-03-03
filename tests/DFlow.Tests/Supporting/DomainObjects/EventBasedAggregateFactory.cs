// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using DFlow.Domain.Aggregates;
using DFlow.Domain.BusinessObjects;
using DFlow.Tests.Supporting.DomainObjects.Commands;

namespace DFlow.Tests.Supporting.DomainObjects
{
    public class EventBasedAggregateFactory: 
        IAggregateFactory<EventStreamBusinessEntityAggregateRoot, AddEntityCommand>,
        IAggregateFactory<EventStreamBusinessEntityAggregateRoot, EventStream<EntityTestId>>
    {
        public EventStreamBusinessEntityAggregateRoot Create(AddEntityCommand command)
        {
            return new EventStreamBusinessEntityAggregateRoot(command.Name, 
                                                                command.Mail, 
                                                                VersionId.New());
        }

        public EventStreamBusinessEntityAggregateRoot Create(EventStream<EntityTestId> source)
        {
            return new EventStreamBusinessEntityAggregateRoot(source);
        }
    }
}
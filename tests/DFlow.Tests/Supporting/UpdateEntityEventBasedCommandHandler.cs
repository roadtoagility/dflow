// Copyright (C) 2022  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using DFlow.Business.Cqrs;
using DFlow.Business.Cqrs.CommandHandlers;
using DFlow.Domain.Aggregates;
using DFlow.Domain.BusinessObjects;
using DFlow.Domain.Events;
using DFlow.Domain.Events.BusinessObjects;
using DFlow.Tests.Supporting.DomainObjects;
using DFlow.Tests.Supporting.DomainObjects.Commands;

namespace DFlow.Tests.Supporting
{
    public sealed class UpdateEntityEventBasedCommandHandler : CommandHandler<UpdateEntityCommand, CommandResult<Guid>>
    {
        private readonly IAggregateFactory<EventStreamBusinessEntityAggregateRoot, EventStream<EntityTestId>>
            _aggregateFactory;
        
        public UpdateEntityEventBasedCommandHandler(IDomainEventBus publisher, 
            IAggregateFactory<EventStreamBusinessEntityAggregateRoot, EventStream<EntityTestId>> aggregateFactory)
            :base(publisher)
        {
            _aggregateFactory = aggregateFactory;
        }
        
        protected override Task<CommandResult<Guid>> ExecuteCommand(UpdateEntityCommand command, CancellationToken cancellationToken)
        {
            var agg = _aggregateFactory.Create(
                EventStream<EntityTestId>.From(EntityTestId.Empty(),
                    new AggregationName(), 
                    VersionId.Empty(), ImmutableList<IDomainEvent>.Empty)
                );
            var isSucceed = agg.IsValid;
            var okId = Guid.Empty;
            
            
            if (isSucceed)
            {
                agg.UpdateName(EntityTestId.From(command.AggregateId), Name.From(command.Name));

                isSucceed = agg.IsValid;
                
                agg.GetEvents().ToImmutableList()
                    .ForEach( ev => Publisher.Publish(ev));
                
                okId = agg.GetChange().AggregationId.Value;
            }
            
            return Task.FromResult(new CommandResult<Guid>(isSucceed, okId,agg.Failures));
        }
    }
}
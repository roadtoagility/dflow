// Copyright (C) 2020  Road to Agility
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Library General Public
// License as published by the Free Software Foundation; either
// version 2 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Library General Public License for more details.
//
// You should have received a copy of the GNU Library General Public
// License along with this library; if not, write to the
// Free Software Foundation, Inc., 51 Franklin St, Fifth Floor,
// Boston, MA  02110-1301, USA.
//

using System;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using DFlow.Business.Cqrs;
using DFlow.Business.Cqrs.CommandHandlers;
using DFlow.Domain.Aggregates;
using DFlow.Domain.BusinessObjects;
using DFlow.Domain.Events;
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
            : base(publisher)
        {
            _aggregateFactory = aggregateFactory;
        }

        protected override Task<CommandResult<Guid>> ExecuteCommand(UpdateEntityCommand command,
            CancellationToken cancellationToken)
        {
            var agg = _aggregateFactory.Create(
                EventStream<EntityTestId>.From(EntityTestId.Empty(),
                    new AggregationName(),
                    VersionId.Empty(), new ImmutableArray<IDomainEvent>())
            );
            var isSucceed = agg.IsValid;
            var okId = Guid.Empty;


            if (isSucceed)
            {
                agg.UpdateName(EntityTestId.From(command.AggregateId), Name.From(command.Name));

                isSucceed = agg.IsValid;

                agg.GetEvents().ToImmutableList()
                    .ForEach(ev => Publisher.Publish(ev));

                okId = agg.GetChange().AggregationId.Value;
            }

            return Task.FromResult(new CommandResult<Guid>(isSucceed, okId, agg.Failures));
        }
    }
}
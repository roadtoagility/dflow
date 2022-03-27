// Copyright (C) 2020  Road to Agility
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
using DFlow.Domain.Events;
using DFlow.Samples.Domain.Aggregates;

namespace DFlow.Samples.Business.CommandHandlers
{
    public sealed class AddUserCommandHandler : CommandHandler<AddUserCommand, CommandResult<Guid>>
    {
        public AddUserCommandHandler(IDomainEventBus publisher)
            : base(publisher)
        {
        }

        protected override Task<CommandResult<Guid>> ExecuteCommand(AddUserCommand command,
            CancellationToken cancellationToken)
        {
            var agg = UserEntityBasedAggregationRoot.CreateFrom(command.Name, command.Mail);

            var isSucceed = agg.IsValid;
            var okId = Guid.Empty;

            if (agg.IsValid)
            {
                agg.GetEvents().ToImmutableList()
                    .ForEach(ev => Publisher.Publish(ev));

                okId = agg.GetChange().Identity.Value;
            }

            return Task.FromResult(new CommandResult<Guid>(isSucceed, okId, agg.Failures));
        }
    }
}
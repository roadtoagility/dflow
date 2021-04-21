﻿// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System;
using System.Collections.Immutable;
using DFlow.Business.Cqrs;
using DFlow.Business.Cqrs.CommandHandlers;
using DFlow.Domain.Events;
using DFlow.Persistence;
using DFlow.Samples.Domain.Aggregates;
using DFlow.Samples.Persistence.Model.Repositories;

namespace DFlow.Samples.Business.CommandHandlers
{
    public sealed class AddUserCommandHandler : CommandHandler<AddUserCommand, CommandResult<Guid>>
    {
        public AddUserCommandHandler(IDomainEventBus publisher)
            :base(publisher)
        {
        }
        
        protected override CommandResult<Guid> ExecuteCommand(AddUserCommand command)
        {
            var agg = UserEntityBasedAggregationRoot.CreateFrom(command.Name,command.Mail);
            
            var isSucceed = agg.ValidationResults.IsValid;
            var okId = Guid.Empty;
      
            if (agg.ValidationResults.IsValid)
            {
                agg.GetEvents().ToImmutableList()
                    .ForEach( ev => Publisher.Publish(ev));
                
                okId = agg.GetChange().Id.Value;
            }
            
            return new CommandResult<Guid>(isSucceed, okId,agg.ValidationResults.Errors.ToImmutableList());
        }
    }
}
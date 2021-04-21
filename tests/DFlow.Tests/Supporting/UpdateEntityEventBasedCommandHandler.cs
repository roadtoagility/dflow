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
using System.Collections.Generic;
using System.Collections.Immutable;
using DFlow.Business.Cqrs;
using DFlow.Business.Cqrs.CommandHandlers;
using DFlow.Domain.BusinessObjects;
using DFlow.Domain.Events;
using DFlow.Tests.Supporting.Commands;
using DFlow.Tests.Supporting.DomainObjects;
using DFlow.Tests.Supporting.DomainObjects.Events;
using Version = DFlow.Domain.BusinessObjects.Version;

namespace DFlow.Tests.Supporting
{
    public sealed class UpdateEntityEventBasedCommandHandler : CommandHandler<UpdateEntityCommand, CommandResult<Guid>>
    {
        public UpdateEntityEventBasedCommandHandler(IDomainEventBus publisher)
            :base(publisher)
        {
        }
        
        protected override CommandResult<Guid> ExecuteCommand(UpdateEntityCommand command)
        {
            // FIXME: remove this after clear my mind, i do need port the persistence event sourcing infrastructure now :)
            var aggOld = EventStreamBusinessEntityAggregateRoot.Create(EntityTestId.GetNext(), 
                Name.From("My name"), Email.From("my@mail.com"));

            var currentstream = aggOld.GetChange();  
            var agg = EventStreamBusinessEntityAggregateRoot.ReconstructFrom(currentstream);
            
            var isSucceed = agg.ValidationResults.IsValid;
            var okId = Guid.Empty;
            
            
            if (isSucceed)
            {
                agg.UpdateName(EntityTestId.From(command.AggregateId), Name.From(command.Name));

                isSucceed = agg.ValidationResults.IsValid;
                
                agg.GetEvents().ToImmutableList()
                    .ForEach( ev => Publisher.Publish(ev));
                
                okId = agg.GetChange().AggregationId.Value;
            }
            
            return new CommandResult<Guid>(isSucceed, okId,agg.ValidationResults.Errors.ToImmutableList());
        }
    }
}
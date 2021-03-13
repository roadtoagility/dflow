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
using DFlow.Business.Cqrs;
using DFlow.Business.Cqrs.CommandHandlers;
using DFlow.Domain.Events;
using DFlow.Tests.Supporting.Commands;
using DFlow.Tests.Supporting.DomainObjects;

namespace DFlow.Tests.Supporting
{
    public sealed class AddEntityEventBasedCommandHandler : CommandHandler<AddEntityCommand, CommandResult<Guid>>
    {
        public AddEntityEventBasedCommandHandler(IDomainEventBus publisher)
            :base(publisher)
        {
        }
        
        protected override CommandResult<Guid> ExecuteCommand(AddEntityCommand command)
        {
            
            var agg = EventStreamBusinessEntityAggregateRoot.Create(EntityTestId.GetNext(), 
                Name.From(command.Name), Email.From(command.Mail));
            
            var isSucceed = false;
            var okId = Guid.Empty;
      
            //validation is not working nice yet
            if (agg.ValidationResults.IsValid)
            {
                isSucceed = true;
                
                agg.GetEvents().ToImmutableList()
                    .ForEach( ev => Publisher.Publish(ev));
                
                okId = agg.GetChange().AggregationId.Value;
            }
            
            return new CommandResult<Guid>(isSucceed, okId,agg.ValidationResults.Errors.ToImmutableList());
        }
    }
}
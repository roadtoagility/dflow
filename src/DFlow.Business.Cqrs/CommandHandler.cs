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
using DFlow.Business.Cqrs.CommandHandlers;
using DFlow.Domain.Events;
using Microsoft.Extensions.Logging;

namespace DFlow.Business.Cqrs
{
    public abstract class CommandHandler<TCommand, TResult> : ICommandHandler<TCommand, TResult>
    {
        private readonly ILogger _logger;
        protected IDomainEventBus Publisher { get; }
        
        protected CommandHandler(ILogger logger, IDomainEventBus publisher)
        {
            _logger = logger;
            Publisher = publisher;
        }

        public TResult Execute(TCommand command)
        {
            try
            {
                return ExecuteCommand(command);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,$"Command Execution Failed with exception {ex.Message}");
                throw;
            }
        }

        protected abstract TResult ExecuteCommand(TCommand command);
    }
}
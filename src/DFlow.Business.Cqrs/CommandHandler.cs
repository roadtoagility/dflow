// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

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
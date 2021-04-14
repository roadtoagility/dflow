// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System;
using System.Threading;
using System.Threading.Tasks;
using DFlow.Business.Cqrs.CommandHandlers;
using DFlow.Domain.Events;
using Microsoft.Extensions.Logging;

namespace DFlow.Business.Cqrs
{
    public abstract class CommandHandler<TCommand, TResult> : 
        ICommandHandler<TCommand, TResult>, 
        ICommandHandlerAsync<TCommand, TResult>
    {
        protected IDomainEventBus Publisher { get; }

        protected CommandHandler(IDomainEventBus publisher)
        {
            Publisher = publisher;
        }
        public TResult Execute(TCommand command)
        {
            return ExecuteCommand(command);
        }
        
        public async Task<TResult> ExecuteAsync(TCommand command)
        {
            var cancellationSource = new CancellationTokenSource();
            return await ExecuteCommandAsync(command, cancellationSource.Token)
                .ConfigureAwait(false);
        }

        protected virtual TResult ExecuteCommand(TCommand command)
        {
            throw new NotImplementedException();
        }

        protected virtual Task<TResult> ExecuteCommandAsync(TCommand command, CancellationToken cancellatioToken)
        {
            throw new NotImplementedException();
        }
    }
}
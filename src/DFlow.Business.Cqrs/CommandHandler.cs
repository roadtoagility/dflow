// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Threading;
using System.Threading.Tasks;
using DFlow.Business.Cqrs.CommandHandlers;
using DFlow.Domain.Events;

namespace DFlow.Business.Cqrs
{
    public abstract class CommandHandler<TCommand, TResult> :
        ICommandHandler<TCommand, TResult>
    {
        protected CommandHandler(IDomainEventBus publisher)
        {
            Publisher = publisher;
        }

        protected IDomainEventBus Publisher { get; }

        public async Task<TResult> Execute(TCommand command)
        {
            var cancellationToken = new CancellationTokenSource();
            return await Execute(command, cancellationToken.Token)
                .ConfigureAwait(false);
        }

        public async Task<TResult> Execute(TCommand command, CancellationToken cancellationToken)
        {
            return await ExecuteCommand(command, cancellationToken)
                .ConfigureAwait(false);
        }

        protected abstract Task<TResult> ExecuteCommand(TCommand command, CancellationToken cancellationToken);
    }
}
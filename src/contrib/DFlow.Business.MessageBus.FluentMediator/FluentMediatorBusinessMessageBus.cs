// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Threading;
using System.Threading.Tasks;
using DFlow.Business.Cqrs;
using FluentMediator;

namespace DFlow.Business.MessageBus.FluentMediator
{
    public class FluentMediatorBusinessMessageBus:IBusinessMessageBus
    {
        private readonly IMediator _mediator;
        
        public FluentMediatorBusinessMessageBus(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        public async Task<TResult> Send<TResult,TRequest>(TRequest request)
        {
            var cancellation = new CancellationToken();
            return await Send<TResult, TRequest>(request, cancellation)
                .ConfigureAwait(false);
        }

        public async Task<TResult> Send<TResult, TRequest>(TRequest request, CancellationToken cancellationToken)
        {
            return await _mediator.SendAsync<TResult>(request, cancellationToken).ConfigureAwait(false);
        }
    }
}
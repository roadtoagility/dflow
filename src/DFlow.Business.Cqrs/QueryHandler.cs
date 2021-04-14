// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System;
using System.Threading;
using System.Threading.Tasks;
using DFlow.Business.Cqrs.QueryHandlers;

namespace DFlow.Business.Cqrs
{
    public abstract class QueryHandler<TFilter, TResult> : IQueryHandler<TFilter, TResult>,
        IQueryHandlerAsync<TFilter, TResult>
    {
        public TResult Execute(TFilter filter)
        {
            return ExecuteQuery(filter);
        }
        
        public async Task<TResult> ExecuteAsync(TFilter filter)
        {
            var cancellationSource = new CancellationTokenSource();
            return await ExecuteQueryAsync(filter,cancellationSource.Token).ConfigureAwait(false);
        }

        protected virtual TResult ExecuteQuery(TFilter filter)
        {
            throw new NotImplementedException();
        }

        protected virtual Task<TResult> ExecuteQueryAsync(TFilter filter, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
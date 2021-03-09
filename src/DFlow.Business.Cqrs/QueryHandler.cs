// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using DFlow.Business.Cqrs.CommandHandlers;

namespace DFlow.Business.Cqrs
{
    public abstract class QueryHandler<TFilter, TResult> : ICommandHandler<TFilter, TResult>
    {
      
        public TResult Execute(TFilter filter)
        {
            return ExecuteQuery(filter);
        }

        protected abstract TResult ExecuteQuery(TFilter filter);
    }
}
// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Collections.Immutable;
using DFlow.Domain.Validation;

namespace DFlow.Business.Cqrs.QueryHandlers
{
    public class QueryResult<TResult> : ExecutionResult
    {
        public QueryResult(bool isSucceed, TResult data)
            : base(isSucceed, ImmutableList<Failure>.Empty)
        {
            Data = data;
        }

        public TResult Data { get; }
    }
}
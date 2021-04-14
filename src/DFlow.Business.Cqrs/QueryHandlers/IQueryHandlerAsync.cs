// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Threading.Tasks;

namespace DFlow.Business.Cqrs.QueryHandlers
{
    public interface IQueryHandlerAsync<in TFilter, TResult>
    {
        Task<TResult> ExecuteAsync(TFilter filter);
    }
}
// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace DFlow.Persistence.Repositories
{
    public interface IRepositoryAppendOnly<TState,TModel>
    {
        void Add(TModel entity);
        Task<IReadOnlyList<TModel>> FindAsync(Expression<Func<TState, bool>> predicate, CancellationToken cancellationToken);
    }
}
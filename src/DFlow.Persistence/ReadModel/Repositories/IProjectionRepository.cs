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

namespace DFlow.Persistence.ReadModel.Repositories
{
    public interface IProjectionRepository<TModel> where TModel : class
    {
        void Add(TModel entity);
        void Remove(TModel entity);
        IReadOnlyList<TModel> Find(Expression<Func<TModel, bool>> predicate);
        Task<IReadOnlyList<TModel>> FindAsync(Expression<Func<TModel, bool>> predicate);

        Task<IReadOnlyList<TModel>> FindAsync(Expression<Func<TModel, bool>> predicate,
            CancellationToken cancellationToken);
    }
}
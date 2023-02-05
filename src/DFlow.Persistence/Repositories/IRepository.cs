// Copyright (C) 2022  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Threading.Tasks;

namespace DFlow.Persistence.Repositories;

public interface IRepository<TState, TModel> : IRepositoryAppendOnly<TState, TModel>
{
    Task Remove(TModel entity);
}

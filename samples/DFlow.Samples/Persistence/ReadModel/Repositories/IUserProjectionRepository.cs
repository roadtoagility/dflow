// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System;
using DFlow.Domain.BusinessObjects;
using DFlow.Persistence.ReadModel.Repositories;

namespace DFlow.Samples.Persistence.ReadModel.Repositories
{
    public interface IUserProjectionRepository : IProjectionRepository<UserProjection>
    {
        UserProjection Get(IIdentity<Guid> entityId);
    }
}
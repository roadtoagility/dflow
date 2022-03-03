// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Collections.Generic;
using DFlow.Business.Cqrs.QueryHandlers;
using DFlow.Samples.Persistence.ReadModel;

namespace DFlow.Samples.Business.QueryHandlers
{
    public class GetUsersResponse:QueryResult<IReadOnlyList<UserProjection>>
    {
        public GetUsersResponse(bool isSucceed, IReadOnlyList<UserProjection> data)
        :base(isSucceed, data)
        {
        }

        public static GetUsersResponse From(bool isSucceed, IReadOnlyList<UserProjection> items)
        {
            return new GetUsersResponse(isSucceed,items);
        }
    }
}
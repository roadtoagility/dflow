// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Threading;
using System.Threading.Tasks;
using DFlow.Business.Cqrs;
using DFlow.Persistence;
using DFlow.Samples.Persistence.ReadModel.Repositories;

namespace DFlow.Samples.Business.QueryHandlers
{
    public sealed class GetUsersByQueryHandler : QueryHandler<GetUsersByFilter, GetUsersResponse>
    {
        private readonly IDbSession<IUserProjectionRepository> _dbSession;

        public GetUsersByQueryHandler(IDbSession<IUserProjectionRepository> session)
        {
            _dbSession = session;
        }

        protected override Task<GetUsersResponse> ExecuteQuery(GetUsersByFilter filter,
            CancellationToken cancellationToken)
        {
            var clients = _dbSession.Repository
                .Find(up => up.Name.Contains(filter.Name));

            return Task.FromResult(GetUsersResponse.From(clients.Count > 0, clients));
        }
    }
}
// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using DFlow.Domain.Events;
using DFlow.Persistence;
using DFlow.Samples.Domain.Aggregates.Events;
using DFlow.Samples.Persistence.ReadModel;
using DFlow.Samples.Persistence.ReadModel.Repositories;

namespace DFlow.Samples.Business.DomainEventHandlers
{
    public sealed class UserAddedUpdateProjectionDomainEventHandler : DomainEventHandler<UserAddedEvent>
    {
        private IDbSession<IUserProjectionRepository> _dbSession;
        public UserAddedUpdateProjectionDomainEventHandler(IDbSession<IUserProjectionRepository> dbSession)
        {
            _dbSession = dbSession;
        }
        protected override void ExecuteHandle(UserAddedEvent @event)
        {
            _dbSession.Repository.Add(new UserProjection(@event.Id.Value, @event.Name.Value, 
                @event.Mail.Value, @event.Version.Value));
            _dbSession.SaveChanges();
        }
    }
}
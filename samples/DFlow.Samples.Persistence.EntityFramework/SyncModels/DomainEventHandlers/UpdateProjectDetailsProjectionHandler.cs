// Copyright (C) 2020  Road to Agility
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Library General Public
// License as published by the Free Software Foundation; either
// version 2 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Library General Public License for more details.
//
// You should have received a copy of the GNU Library General Public
// License along with this library; if not, write to the
// Free Software Foundation, Inc., 51 Franklin St, Fifth Floor,
// Boston, MA  02110-1301, USA.
//


using AppFabric.Domain.AggregationProject.Events;
using AppFabric.Domain.Framework.DomainEvents;
using AppFabric.Persistence.Framework;
using AppFabric.Persistence.ReadModel;
using AppFabric.Persistence.ReadModel.Repositories;

namespace AppFabric.Persistence.SyncModels.DomainEventHandlers
{
    public class UpdateProjectDetailsProjectionHandler : DomainEventHandler<ProjectDetailUpdatedEvent>
    {
        private readonly IDbSession<IProjectProjectionRepository> _projectSession;

        public UpdateProjectDetailsProjectionHandler(IDbSession<IProjectProjectionRepository> projectSession)
        {
            _projectSession = projectSession;
        }

        protected override void ExecuteHandle(ProjectDetailUpdatedEvent @event)
        {
            var project = _projectSession.Repository.Get(@event.Id);
            
            var projection = new ProjectProjection(
                project.Id,
                @event.Name.Value,
                project.Code,
                @event.Budget.Value,
                project.StartDate,
                project.ClientId,
                project.ClientName,
                @event.Owner.Value,
                @event.OrderNumber.Value,
                project.Status,
                project.StatusName,
                @event.Version.Value);
            
            _projectSession.Repository.Add(projection);
            _projectSession.SaveChanges();
        }
    }
}
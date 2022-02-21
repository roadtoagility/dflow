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


using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AppFabric.Domain.Framework.BusinessObjects;
using Version = AppFabric.Domain.BusinessObjects.Version;

namespace AppFabric.Persistence.ReadModel.Repositories
{
    public sealed class ProjectProjectionRepository : IProjectProjectionRepository
    {
        private readonly AppFabricDbContext _context;
        public ProjectProjectionRepository(AppFabricDbContext context)
        {
            _context = context;
        }

        public ProjectProjection Get(EntityId id)
        {
            var project = _context.ProjectsProjection
                .FirstOrDefault(ac => ac.Id.Equals(id.Value));
            
            if (project == null)
            {
                ProjectProjection.Empty();
            }
            
            return project;
        }

        public void Add(ProjectProjection entity)
        {
            var oldState =
                _context.ProjectsProjection
                    .FirstOrDefault(b => b.Id == entity.Id);

            if (oldState == null)
            {
                _context.ProjectsProjection.Add(entity);
            }
            else
            {
                _context.Entry(oldState).CurrentValues.SetValues(entity);
            }
        }

        public void Remove(ProjectProjection entity)
        {
            _context.ProjectsProjection.Remove(entity);
        }

        public IReadOnlyList<ProjectProjection> Find(Expression<Func<ProjectProjection, bool>> predicate)
        {
            return _context.ProjectsProjection.Where(predicate).ToList();
        }
    }
}
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
using Microsoft.EntityFrameworkCore;
using AppFabric.Domain.BusinessObjects;
using AppFabric.Domain.Framework.BusinessObjects;
using AppFabric.Persistence.ExtensionMethods;
using Version = AppFabric.Domain.BusinessObjects.Version;

namespace AppFabric.Persistence.Model.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        public ProjectRepository(AppFabricDbContext context)
        {
            DbContext = context;
        }

        private AppFabricDbContext DbContext { get; }

        // https://docs.microsoft.com/en-us/ef/core/saving/disconnected-entities

        public void Add(Project entity)
        {
            var entry = entity.ToProjectState();
            var oldState = DbContext.Projects
                .OrderByDescending(ob => ob.Id)
                .ThenByDescending(ob => ob.RowVersion)
                .FirstOrDefault(t => t.Id == entity.Id.Value);

            if (oldState == null)
            {
                DbContext.Projects.Add(entry);
            }
            else
            {
                var version = Version.From(BitConverter.ToInt32(oldState.RowVersion));
                
                if (Version.Next(version) > entity.Version)
                {
                    throw new DbUpdateConcurrencyException("This version is not the most updated for this object.");
                }

                DbContext.Entry(oldState).CurrentValues.SetValues(entry);
            }
        }

        public void Remove(Project entity)
        {
            var oldState = Get(entity.Id);

            if (Version.Next(oldState.Version) > entity.Version)
            {
                throw new DbUpdateConcurrencyException("This version is not the most updated for this object.");
            }
            
            var entry = entity.ToProjectState();

            DbContext.Projects.Remove(entry);
        }

        public Project Get(EntityId id)
        {
            var project = DbContext.Projects.AsNoTracking()
                .OrderByDescending(ob => ob.Id)
                .ThenByDescending(ob => ob.RowVersion)
                .FirstOrDefault(t => t.Id == id.Value);
            
            if (project == null)
            {
                return Project.Empty();
            }
            
            return project.ToProject();
        }

        public IEnumerable<Project> Find(Expression<Func<ProjectState, bool>> predicate)
        {
            return DbContext.Projects.Where(predicate).AsNoTracking()
                .Select(t =>  t.ToProject());
            ;
        }
    }
}
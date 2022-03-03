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
    public class UserRepository : IUserRepository
    {
        public UserRepository(AppFabricDbContext context)
        {
            DbContext = context;
        }

        private AppFabricDbContext DbContext { get; }

        // https://docs.microsoft.com/en-us/ef/core/saving/disconnected-entities

        public void Add(User entity)
        {
            var entry = entity.ToUserState();

            var oldState = Get(entity.Id);

            if (oldState.Equals(User.Empty()))
            {
                DbContext.Users.Add(entry);
            }
            else
            {
                if (Version.Next(oldState.Version) > entity.Version)
                {
                    throw new DbUpdateConcurrencyException("This version is not the most updated for this object.");
                }

                DbContext.Entry(oldState).CurrentValues.SetValues(entry);
            }
        }

        public void Remove(User entity)
        {
            var oldState = Get(entity.Id);

            if (Version.Next(oldState.Version) > entity.Version)
            {
                throw new DbUpdateConcurrencyException("This version is not the most updated for this object.");
            }
            
            var entry = entity.ToUserState();
            
            DbContext.Users.Remove(entry);
        }

        public User Get(EntityId id)
        {
            var user = DbContext.Users.AsNoTracking()
                .OrderByDescending(ob => ob.Id)
                .ThenByDescending(ob => ob.RowVersion)
                .FirstOrDefault(t =>t.Id.Equals(id.Value));
            
            if (user == null)
            {
                return User.Empty();
            }
            
            return user.ToUser();
        }

        public IEnumerable<User> Find(Expression<Func<UserState, bool>> predicate)
        {
            return DbContext.Users.Where(predicate).AsNoTracking()
                .Select(t =>  t.ToUser());
            ;
        }
    }
}
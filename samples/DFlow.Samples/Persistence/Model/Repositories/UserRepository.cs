// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using DFlow.Domain.BusinessObjects;
using DFlow.Samples.Domain.BusinessObjects;
using DFlow.Samples.Persistence.ExtensionMethods;
using Microsoft.EntityFrameworkCore;

namespace DFlow.Samples.Persistence.Model.Repositories
{
    public class UserRepository : IUserRepository
    {
        public UserRepository(SampleAppDbContext context)
        {
            DbContext = context;
            
            DbContext.Database.EnsureDeleted();
            DbContext.Database.EnsureCreated();
        }

        private SampleAppDbContext DbContext { get; }

        // https://docs.microsoft.com/en-us/ef/core/saving/disconnected-entities

        public async Task Add(User entity)
        {
            var entry = entity.ToUserState();

            var oldState = await Get(entity);

            if (oldState.Equals(User.Empty()))
            {
                DbContext.Users.Add(entry);
            }
            else
            {
                if (VersionId.Next(oldState.Version) > entity.Version)
                {
                    throw new DbUpdateConcurrencyException("This version is not the most updated for this object.");
                }

                DbContext.Entry(oldState).CurrentValues.SetValues(entry);
            }
        }

        public async Task Remove(User entity)
        {
            var oldState = await Get(entity);

            if (VersionId.Next(oldState.Version) > entity.Version)
            {
                throw new DbUpdateConcurrencyException("This version is not the most updated for this object.");
            }

            var entry = entity.ToUserState();
            
            DbContext.Users.Remove(entry);
        }

        public async Task<User> Get(IEntityIdentity<EntityId> id)
        {
            var user = await DbContext.Users.AsNoTracking()
                .OrderByDescending(ob => ob.Id)
                .ThenByDescending(ob => ob.RowVersion)
                .FirstOrDefaultAsync(t =>t.Id.Equals(id.Identity.Value))
                .ConfigureAwait(false);
            
            if (user == null)
            {
                return User.Empty();
            }
            
            return user.ToUser();
        }
       
        public async Task<IReadOnlyList<User>> FindAsync(Expression<Func<UserState, bool>> predicate, CancellationToken cancellationToken)
        {
            return await DbContext.Users.Where(predicate).AsNoTracking()
                .Select(t => t.ToUser()).ToListAsync(cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
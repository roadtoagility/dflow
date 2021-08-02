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
using Version = DFlow.Domain.BusinessObjects.Version;

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

        public void Add(User entity)
        {
            var entry = entity.ToUserState();

            var oldState = Get(entity);

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
            var oldState = Get(entity);

            if (Version.Next(oldState.Version) > entity.Version)
            {
                throw new DbUpdateConcurrencyException("This version is not the most updated for this object.");
            }

            var entry = entity.ToUserState();
            
            DbContext.Users.Remove(entry);
        }

        public User Get(IEntityIdentity<Guid> id)
        {
            var user = DbContext.Users.AsNoTracking()
                .OrderByDescending(ob => ob.Id)
                .ThenByDescending(ob => ob.RowVersion)
                .FirstOrDefault(t =>t.Id.Equals(id.Identity));
            
            if (user == null)
            {
                return User.Empty();
            }
            
            return user.ToUser();
        }
        public IEnumerable<User> Find(Expression<Func<UserState, bool>> predicate)
        {
            return DbContext.Users.Where(predicate).AsNoTracking()
                .Select(t => t.ToUser()).ToList();
        }

        
        public async Task<IEnumerable<User>> FindAsync(Expression<Func<UserState, bool>> predicate)
        {
            var cancellationToken = new CancellationToken();
            return await FindAsync(predicate, cancellationToken)
                .ConfigureAwait(false);
        }
        public async Task<IEnumerable<User>> FindAsync(Expression<Func<UserState, bool>> predicate, CancellationToken cancellationToken)
        {
            return await DbContext.Users.Where(predicate).AsNoTracking()
                .Select(t => t.ToUser()).ToListAsync()
                .ConfigureAwait(false);
        }
    }
}
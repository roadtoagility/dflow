// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using DFlow.Domain.BusinessObjects;
using DFlow.Samples.Domain.Aggregates.Events;
using LiteDB;

namespace DFlow.Samples.Persistence.ReadModel.Repositories
{
    public sealed class UserProjectionRepository : IUserProjectionRepository
    {
        private readonly SampleAppProjectionDbContext _context;
        public UserProjectionRepository(SampleAppProjectionDbContext context)
        {
            _context = context;
        }

        public UserProjection Get(IEntityIdentity<Guid> id)
        {
            var user = _context.Users.FindById(id.Identity);
            
            if (user == null)
            {
                return UserProjection.Empty();
            }
            
            return user;
        }

        public void Add(UserProjection entity)
        {
            _context.Users.Upsert(entity);
        }

        public void Remove(UserProjection entity)
        {
            _context.Users.Delete(entity.Id);
        }

        public Task<IReadOnlyList<UserProjection>> FindAsync(Expression<Func<UserProjection, bool>> predicate, CancellationToken cancellationToken)
        {
            return Task.FromResult<IReadOnlyList<UserProjection>>(_context.Users.FindAll()
                .Where(predicate.Compile()).ToImmutableList());
        }
    }
}
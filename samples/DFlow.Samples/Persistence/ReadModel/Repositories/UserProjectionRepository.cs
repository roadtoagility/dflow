// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DFlow.Domain.BusinessObjects;

namespace DFlow.Samples.Persistence.ReadModel.Repositories
{
    public sealed class UserProjectionRepository : IUserProjectionRepository
    {
        private readonly SampleAppDbContext _context;
        public UserProjectionRepository(SampleAppDbContext context)
        {
            _context = context;
        }

        public UserProjection Get(IIdentity<Guid> id)
        {
            var user = _context.UsersProjection
                .FirstOrDefault(ac => ac.Id.Equals(id.Value));
            
            if (user == null)
            {
                return UserProjection.Empty();
            }
            
            return user;
        }

        public void Add(UserProjection entity)
        {
            var oldState =
                _context.UsersProjection.FirstOrDefault(b => b.Id == entity.Id);

            if (oldState == null)
            {
                _context.UsersProjection.Add(entity);
            }
            else
            {
                _context.Entry(oldState).CurrentValues.SetValues(entity);
            }
        }

        public void Remove(UserProjection entity)
        {
            _context.UsersProjection.Remove(entity);
        }

        public IReadOnlyList<UserProjection> Find(Expression<Func<UserProjection, bool>> predicate)
        {
            return _context.UsersProjection.Where(predicate).ToList();
        }
    }
}
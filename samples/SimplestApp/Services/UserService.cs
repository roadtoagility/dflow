// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System;
using DFlow.Domain.Specifications;
using DFlow.Samples.Domain.Aggregates;
using DFlow.Samples.Domain.BusinessObjects;
using SimplestApp.Operations;

namespace SimplestApp.Services
{
    public class UserService:IUserService
    {
        private ISpecification<UserEntityBasedAggregationRoot> _specification;
        public UserService(ISpecification<UserEntityBasedAggregationRoot> specification)
        {
            _specification = specification;
        }
        
        public User Add(AddUser user)
        {
            var agg = UserEntityBasedAggregationRoot.CreateFrom(Name.From(user.Name), Email.From(user.Mail));

            if (!_specification.IsSatisfiedBy(agg))
            {
                throw new ArgumentException(agg.Failures[0].Message);
            }

            return agg.GetChange();
        }
    }
}
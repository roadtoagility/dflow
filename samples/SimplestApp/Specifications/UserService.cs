// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System;
using DFlow.Domain.Specifications;
using DFlow.Domain.Validation;
using DFlow.Samples.Domain.Aggregates;
using DFlow.Samples.Domain.BusinessObjects;
using SimplestApp.Operations;

namespace SimplestApp.Specifications
{
    public class UserValidSpecification : CompositeSpecification<UserEntityBasedAggregationRoot>
    {
        public User Add(AddUser user)
        {
            var agg = UserEntityBasedAggregationRoot.CreateFrom(Name.From(user.Name), Email.From(user.Mail));

            if (!agg.IsValid)
                throw new ArgumentException("One or more parameters informed to create a user are not valid.");

            return agg.GetChange();
        }

        public override bool IsSatisfiedBy(UserEntityBasedAggregationRoot candidate)
        {
            if (!candidate.IsValid)
            {
                candidate.AppendValidationResult(Failure.For("InvalidUser",
                    "One or more parameters informed to create a user are not valid."));
                return false;
            }

            return true;
        }
    }
}
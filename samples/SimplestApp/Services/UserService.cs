// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System;
using DFlow.Samples.BusinessObjects.BusinessObjects;
using DFlow.Samples.BusinessObjects.Domain.BusinessObjects;
using DFlow.Samples.Domain.Aggregates;
using DFlow.Samples.Domain.BusinessObjects;
using SimplestApp.Operations;

namespace SimplestApp.Services
{
    public class UserService:IUserService
    {
        public User Add(AddUser user)
        {
            var agg = UserEntityBasedAggregationRoot.CreateFrom(Name.From(user.Name), Email.From(user.Mail));

            if (!agg.ValidationResults.IsValid)
            {
                throw new ArgumentException("One or more parameters informed to create a user are not valid.");
            }

            return agg.GetChange();
        }
    }
}
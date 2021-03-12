// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using DFlow.Domain.Aggregates;
using DFlow.Domain.BusinessObjects;
using DFlow.Samples.BusinessObjects.BusinessObjects;

namespace DFlow.Samples.BusinessObjects.Aggregates
{
    public sealed class UserObjectBasedAggregationRoot : ObjectBasedAggregationRoot<User>
    {

        private UserObjectBasedAggregationRoot(User user)
        {
            if (user.ValidationResults.IsValid)
            {
                Apply(user);
            }

            ValidationResults = user.ValidationResults;
        }

        #region Aggregation contruction
       
        public static UserObjectBasedAggregationRoot CreateFrom(Name name, Email commercialEmail)
        {
            var user = User.From(EntityId.GetNext(), name, commercialEmail, Version.New());
            return new UserObjectBasedAggregationRoot(user);
        }

        #endregion
    }
}
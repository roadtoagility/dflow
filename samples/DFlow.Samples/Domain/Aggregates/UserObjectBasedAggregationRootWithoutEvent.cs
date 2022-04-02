// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using DFlow.Domain.Aggregates;
using DFlow.Domain.BusinessObjects;
using DFlow.Samples.Domain.BusinessObjects;

namespace DFlow.Samples.Domain.Aggregates
{
    public sealed class UserObjectBasedAggregationRootWithoutEvent : ObjectBasedAggregationRoot<User, EntityId>
    {
        private UserObjectBasedAggregationRootWithoutEvent(User user)
        {
            if (user.IsValid) Apply(user);

            AppendValidationResult(user.Failures);
        }

        #region Aggregation contruction

        public static UserObjectBasedAggregationRootWithoutEvent CreateFrom(Name name, Email commercialEmail)
        {
            var user = User.From(EntityId.GetNext(), name, commercialEmail, VersionId.New());
            return new UserObjectBasedAggregationRootWithoutEvent(user);
        }

        #endregion
    }
}
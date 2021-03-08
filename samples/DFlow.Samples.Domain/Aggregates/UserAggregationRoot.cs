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

using AppFabric.Domain.AggregationProject;
using AppFabric.Domain.AggregationProject.Events;
using AppFabric.Domain.AggregationUser.Events;
using AppFabric.Domain.BusinessObjects;
using AppFabric.Domain.Framework.Aggregates;
using AppFabric.Domain.Framework.BusinessObjects;

namespace AppFabric.Domain.AggregationUser
{
    public sealed class UserAggregationRoot : AggregationRoot<User>
    {

        private UserAggregationRoot(User user)
        {
            if (user.ValidationResults.IsValid)
            {
                Apply(user);
                
                if (user.IsNew())
                {
                    Raise(UserAddedEvent.For(user));
                }
            }

            ValidationResults = user.ValidationResults;
        }

        #region Aggregation contruction

        
        public static UserAggregationRoot ReconstructFrom(User currentState)
        {
            var nextVersion = currentState.ValidationResults.IsValid?
                Version.Next(currentState.Version):currentState.Version;
            var user = User.From(currentState.Id, currentState.Name, currentState.Cnpj,
                currentState.CommercialEmail, nextVersion);
            
            return new UserAggregationRoot(user);

        }

        
        public static UserAggregationRoot CreateFrom(Name name, SocialSecurityId cnpj, Email commercialEmail)
        {
            var user = User.From(EntityId.GetNext(), name, cnpj, commercialEmail, Version.New());
            return new UserAggregationRoot(user);
        }

        #endregion

        public void Remove()
        {
            if (ValidationResults.IsValid)
            {
                Raise(UserRemovedEvent.For(this.GetChange()));
            }
        }
    }
}
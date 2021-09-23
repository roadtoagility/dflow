// Copyright (C) 2021  Road to Agility
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


using DFlow.Domain.BusinessObjects;
using DFlow.Domain.Specifications;
using DFlow.Tests.Supporting.DomainObjects;

namespace DFlow.Tests.Supporting.Specifications
{
    public sealed class AnotherBusinessEntityNameIsRoad : CompositeSpecification<AnotherBusinessEntity>
    {
        private readonly Name _nameToCheck;

        public AnotherBusinessEntityNameIsRoad(Name nameToCheck)
        {
            _nameToCheck = nameToCheck;
        }
        
        public override bool IsSatisfiedBy(AnotherBusinessEntity candidate)
        {
            if (candidate.EntityName != _nameToCheck )
            {
                AppendUnsatisfiedRule(UnsatisfiedRule.For("EntityName",$"The canditate name {candidate.EntityName} it is not spected name."));
                return false;
            }
            
            return true;
        }
    }
}
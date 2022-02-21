// Copyright (C) 2021  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace DFlow.Domain.Specifications
{
    public class OrSpecification<TBusinessObject>:LogicalSpecification<TBusinessObject>
    {
        public OrSpecification(ISpecification<TBusinessObject> leftCondition, ISpecification<TBusinessObject> rightCondition) 
            : base(leftCondition, rightCondition)
        {
        }

        public override bool IsSatisfiedBy(TBusinessObject candidate)
        {
            return LeftCondition.IsSatisfiedBy(candidate) 
                   || RightCondition.IsSatisfiedBy(candidate);
        }
    }
}

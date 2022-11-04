// Copyright (C) 2022  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace DFlow.Specifications
{
    public abstract class LogicalSpecification<TBusinessObject>:CompositeSpecification<TBusinessObject>
    {
        protected ISpecification<TBusinessObject> LeftCondition { get; }

        protected ISpecification<TBusinessObject> RightCondition { get; }

        protected LogicalSpecification(ISpecification<TBusinessObject> leftCondition,
            ISpecification<TBusinessObject> rightCondition)
        {
            LeftCondition = leftCondition;
            RightCondition = rightCondition;
        }
    }
}

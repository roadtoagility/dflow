// Copyright (C) 2021  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace DFlow.Specifications
{
    public class NotSpecification<TBusinessObject>:CompositeSpecification<TBusinessObject>
    {
        private readonly ISpecification<TBusinessObject> _condition;
        
        public NotSpecification(ISpecification<TBusinessObject> condition)
        {
            _condition = condition;
        }

        public override bool IsSatisfiedBy(TBusinessObject candidate)
        {
            return !_condition.IsSatisfiedBy(candidate);
        }
    }
}

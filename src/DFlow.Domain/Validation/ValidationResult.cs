// Copyright (C) 2022  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace DFlow.Domain.Validation
{
    public class ValidationResult
    {
        private readonly List<Failure> _failures = new();

        public ValidationResult( IReadOnlyList<Failure> failures)
        {
            if (failures.Count > 0)
            {
                _failures.AddRange(failures);                
            }
        }

        public ValidationResult(Failure failure)
        :this(new List<Failure>(){failure})
        {
        }
        
        public virtual bool IsValid => Failures.Count == 0;

        public IReadOnlyList<Failure> Failures => _failures;

        public void Append(Failure failure)
        {
            _failures.Add(failure);
        }
        
        public static ValidationResult For(IReadOnlyList<Failure> failures)
        {
            return new ValidationResult(failures);
        } 
        
        public static ValidationResult For(Failure failure)
        {
            return new ValidationResult(failure);
        } 
        
        public static ValidationResult Empty()
        {
            return new ValidationResult();
        }
    }
}
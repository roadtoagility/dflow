// Copyright (c) 2021 Harry McIntyre
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies
// of the Software, and to permit persons to whom the Software is furnished to do so,
// subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A
// PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
// SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.


using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using FluentValidation;
using FluentValidation.Results;

namespace DFlow.Domain.BusinessObjects
{
    public class ValueOf<TValue, TThis, TValidator> 
        where TThis : ValueOf<TValue, TThis, TValidator>
        where TValidator: AbstractValidator<TThis>, new()
    {
        public ValidationResult ValidationStatus { get; private set; }

        private static readonly Func<TThis> Factory;

        /// <summary>
        /// WARNING - THIS FEATURE IS EXPERIMENTAL. I may change it to do
        /// validation in a different way.
        /// Right now, override this method, and throw any exceptions you need to.
        /// Access this.Value to check the value
        /// </summary>
        protected virtual void Validate()
        {
            ValidationStatus = new TValidator().Validate((TThis)this);
        }

        static ValueOf()
        {
            ConstructorInfo ctor = typeof(TThis)
                .GetTypeInfo()
                .DeclaredConstructors
                .First();

            var argsExp = new Expression[0];
            NewExpression newExp = Expression.New(ctor, argsExp);
            LambdaExpression lambda = Expression.Lambda(typeof(Func<TThis>), newExp);

            Factory = (Func<TThis>)lambda.Compile();
        }

        public TValue Value { get; protected set; }

        public static TThis From(TValue item)
        {
            TThis x = Factory();
            x.Value = item;
            x.Validate();

            return x;
        }

        protected virtual bool Equals(ValueOf<TValue, TThis,TValidator> other)
        {
            return EqualityComparer<TValue>.Default.Equals(Value, other.Value);
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            return obj.GetType() == GetType() && Equals((ValueOf<TValue, TThis, TValidator>)obj);
        }

        public override int GetHashCode()
        {
            return EqualityComparer<TValue>.Default.GetHashCode(Value);
        }

        public static bool operator ==(ValueOf<TValue, TThis, TValidator> a, ValueOf<TValue, TThis, TValidator> b)
        {
            if (a is null && b is null)
                return true;

            if (a is null || b is null)
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(ValueOf<TValue, TThis, TValidator> a, ValueOf<TValue, TThis, TValidator> b)
        {
            return !(a == b);
        }

        // Implicit operator removed. See issue #14.

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
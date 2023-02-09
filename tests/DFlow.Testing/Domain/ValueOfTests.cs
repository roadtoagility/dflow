// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.


using DFlow.BusinessObjects;
using DFlow.Validation;
using Xunit;

namespace DFlow.Testing.Domain
{
    public sealed class ValueOfTests
    {
        [Fact]
        public void ValueOf_create_Vo_isValid()
        {
            var vo = SimpleVo.From("teste");

            Assert.True(vo.ValidationStatus.IsValid);
        }

        [Fact]
        public void ValueOf_create_Vo_not_Valid()
        {
            var vo = SimpleVo.From("");
            Assert.True(vo.ValidationStatus.IsValid);
        }
        
        [Fact]
        public void ValueOf_create_Vo_is_the_same()
        {
            var vo1 = SimpleVo.From("testes");
            var vo2 = SimpleVo.From("testes");

            Assert.True(vo1.Equals(vo2));
        }
        
        [Fact]
        public void ValueOf_create_Vo_is_not_the_same()
        {
            var vo1 = SimpleVo.From("testes 1");
            var vo2 = SimpleVo.From("testes");

            Assert.False(vo1.Equals(vo2));
        }
        
        [Fact]
        public void ValueOf_create_ComplexVo_isValid()
        {
            var vo = ComplexVo.From(("teste", SimpleVo.From("outro vo")));

            Assert.True(vo.ValidationStatus.IsValid);
        }

        [Fact]
        public void ValueOf_create_ComplexVo_not_Valid()
        {
            var vo = ComplexVo.From(("", SimpleVo.From("")));

            Assert.False(vo.ValidationStatus.IsValid);
        }

        [Fact]
        public void ValueOf_create_ComplexVoNamedTuple_Valid()
        {
            var vo = ComplexVo.From(("", SimpleVo.From("")));

            Assert.False(vo.ValidationStatus.IsValid);
        }
        
        class ComplexNamedTupleVo : ValueOf<(string Name,SimpleVo Svo), ComplexVo>
        {
            protected override void Validate()
            {
                if (string.IsNullOrEmpty(Value.Name))
                {
                    ValidationStatus.Append(Failure.For("Name", "Name can't be empty"));
                }
                
                if (Value.Svo.Equals(null))
                {
                    ValidationStatus.Append(Failure.For("Svo", "Name can't be empty"));
                }
            }
        }
        
        class ComplexVo : ValueOf<(string,SimpleVo), ComplexVo>
        {
            protected override void Validate()
            {
                if (string.IsNullOrEmpty(Value.Item1))
                {
                    ValidationStatus.Append(Failure.For("Name", "Name can't be empty"));
                }
                
                if (Value.Item2.Equals(null))
                {
                    ValidationStatus.Append(Failure.For("Svo", "Name can't be empty"));
                }
            }
            
        }
        
        class SimpleVo : ValueOf<string, SimpleVo>
        {
        }
    }
}
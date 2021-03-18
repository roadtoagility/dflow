using System;
using BenchmarkDotNet.Attributes;
using DFlow.Tests.Supporting.DomainObjects;

namespace DFlow.Profiling.Domain
{
    [MemoryDiagnoser]
    public class ValueObjectCreateBenchmarks
    {
        private Guid _id = Guid.NewGuid();
        
        [Benchmark(Baseline = true)]
        public void EntityTestIdCreateEmpty()
        {
            var entity = EntityTestId.Empty();
        }
        
        [Benchmark]
        public void EntityTestIdCreateFrom()
        {
            var entity = EntityTestId.From(_id);
        }
    }
}
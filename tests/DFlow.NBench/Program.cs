using System;
using BenchmarkDotNet.Running;
using DFlow.Profiling.Domain;

namespace DFlow.Profiling
{
    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<ValueObjectCreateBenchmarks>();
        }
    }
}
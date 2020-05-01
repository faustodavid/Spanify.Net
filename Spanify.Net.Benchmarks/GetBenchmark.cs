using System;
using System.Linq;
using BenchmarkDotNet.Attributes;

namespace Spanify.Net.Benchmarks
{
    [MemoryDiagnoser]
    public class GetBenchmark
    {
        private string _payload;
        
        [Params(1, 5, 10, 20, 100)] 
        public int N { get; set; }

        [GlobalSetup]
        public void GlobalSetup()
        {
            _payload = string.Join(',', Enumerable.Range(0, N).Select(n => Guid.NewGuid()));
        }
        
        [Benchmark(Baseline = true)]
        public int Get() => _payload.Split(',')[N/2].Length;
        
        [Benchmark]
        public int SpanifyGet() => _payload.AsSpan().Split(',')[N/2].Length;
    }
}
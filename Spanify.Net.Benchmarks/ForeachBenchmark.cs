using System;
using System.Linq;
using BenchmarkDotNet.Attributes;

namespace Spanify.Net.Benchmarks
{
    [MemoryDiagnoser]
    public class ForeachBenchmark
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
        public int Split()
        {
            int count = 0;
            foreach (var value in _payload.Split(','))
            {
                count += value.Length;
            }

            return count;
        }
        
        [Benchmark]
        public int SpanifySplit()
        {
            int count = 0;
            foreach (var value in _payload.AsSpan().Split(','))
            {
                count += value.Length;
            }

            return count;
        }
    }
}
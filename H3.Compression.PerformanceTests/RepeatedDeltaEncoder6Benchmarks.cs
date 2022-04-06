using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnostics.Windows.Configs;

namespace H3.Compression.PerformanceTests
{
    [MemoryDiagnoser]
    [EtwProfiler]
    public class RepeatedDeltaEncoder6Benchmarks
    {
        readonly byte[] _data;
        readonly RepeatedDelta _repeatedDelta;
        public RepeatedDeltaEncoder6Benchmarks()
        {
            _repeatedDelta = new RepeatedDelta(63, ulong.MaxValue);
            _data = RepeatedDeltaEncoder6.Encode(_repeatedDelta);
        }

        [Benchmark]
        public ReadOnlySpan<byte> Encode() => RepeatedDeltaEncoder6.Encode(_repeatedDelta);
        
        [Benchmark]
        public ReadOnlySpan<byte> Decode() => RepeatedDeltaEncoder6.Decode(_data, out _);
    }
}
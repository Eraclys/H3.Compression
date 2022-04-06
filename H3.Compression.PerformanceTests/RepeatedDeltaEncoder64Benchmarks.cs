using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnostics.Windows.Configs;

namespace H3.Compression.PerformanceTests
{
    [MemoryDiagnoser]
    [EtwProfiler]
    public class RepeatedDeltaEncoder64Benchmarks
    {
        readonly byte[] _data;
        readonly RepeatedDelta _repeatedDelta;
        public RepeatedDeltaEncoder64Benchmarks()
        {
            _repeatedDelta = new RepeatedDelta(ulong.MaxValue, ulong.MaxValue);
            _data = RepeatedDeltaEncoder64.Encode(_repeatedDelta);
        }

        [Benchmark]
        public ReadOnlySpan<byte> Encode() => RepeatedDeltaEncoder64.Encode(_repeatedDelta);
        
        [Benchmark]
        public ReadOnlySpan<byte> Decode() => RepeatedDeltaEncoder64.Decode(_data, out _);
    }
}
using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnostics.Windows.Configs;

namespace H3.Compression.PerformanceTests
{
    [MemoryDiagnoser]
    [EtwProfiler]
    public class RepeatedDeltaEncoder2Benchmarks
    {
        readonly byte[] _data;
        readonly RepeatedDelta _repeatedDelta;
        public RepeatedDeltaEncoder2Benchmarks()
        {
            _repeatedDelta = new RepeatedDelta(3, ulong.MaxValue);
            _data = RepeatedDeltaEncoder2.Encode(_repeatedDelta);
        }

        [Benchmark]
        public ReadOnlySpan<byte> Encode() => RepeatedDeltaEncoder2.Encode(_repeatedDelta);
        
        [Benchmark]
        public ReadOnlySpan<byte> Decode() => RepeatedDeltaEncoder2.Decode(_data, out _);
    }
}
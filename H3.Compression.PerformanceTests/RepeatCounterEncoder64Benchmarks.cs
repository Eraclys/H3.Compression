using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnostics.Windows.Configs;

namespace H3.Compression.PerformanceTests
{
    [MemoryDiagnoser]
    [EtwProfiler]
    public class RepeatCounterEncoder64Benchmarks
    {
        readonly byte[] _data;
        
        public RepeatCounterEncoder64Benchmarks()
        {
            _data = RepeatCountEncoder.Encode(ulong.MaxValue);
        }

        [Benchmark]
        public ReadOnlySpan<byte> Encode() => RepeatCountEncoder.Encode(ulong.MaxValue);
        
        [Benchmark]
        public ReadOnlySpan<byte> Decode() => RepeatCountEncoder.Decode(_data, out _);
    }
}
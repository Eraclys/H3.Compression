using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnostics.Windows.Configs;

namespace H3.Compression.PerformanceTests
{
    [MemoryDiagnoser]
    [EtwProfiler]
    public class ConstantEncoder6Benchmarks
    {
        readonly byte[] _data;
        
        public ConstantEncoder6Benchmarks()
        {
            _data = new[] { ConstantEncoder6.Encode(63) };
        }

        [Benchmark]
        public byte Encode() => ConstantEncoder6.Encode(63);
        
        [Benchmark]
        public ReadOnlySpan<byte> Decode() => ConstantEncoder6.Decode(_data, out _);
    }
}
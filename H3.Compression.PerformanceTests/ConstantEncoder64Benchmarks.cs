using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnostics.Windows.Configs;

namespace H3.Compression.PerformanceTests
{
    [MemoryDiagnoser]
    [EtwProfiler]
    public class ConstantEncoder64Benchmarks
    {
        readonly byte[] _data;
        
        public ConstantEncoder64Benchmarks()
        {
            _data = ConstantEncoder64.Encode(63);
        }

        [Benchmark]
        public ReadOnlySpan<byte> Encode() => ConstantEncoder64.Encode(63);
        
        [Benchmark]
        public ReadOnlySpan<byte> Decode() => ConstantEncoder64.Decode(_data, out _);
    }
}
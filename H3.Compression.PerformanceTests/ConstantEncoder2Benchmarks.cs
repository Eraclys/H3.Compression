using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnostics.Windows.Configs;

namespace H3.Compression.PerformanceTests
{
    [MemoryDiagnoser]
    [EtwProfiler]
    public class ConstantEncoder2Benchmarks
    {
        readonly byte[] _data;
        
        public ConstantEncoder2Benchmarks()
        {
            _data = new[] { ConstantEncoder2.Encode(3, 15) };
        }

        [Benchmark]
        public byte Encode() => ConstantEncoder2.Encode(3, 15);
        
        [Benchmark]
        public ReadOnlySpan<byte> Decode() => ConstantEncoder2.Decode(_data, out _, out _);
    }
}
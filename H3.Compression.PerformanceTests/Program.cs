using BenchmarkDotNet.Running;

namespace H3.Compression.PerformanceTests
{
    public class Program
    {
        public static void Main()
        {
            BenchmarkRunner.Run<RepeatedDeltaEncoder64Benchmarks>();
        }
    }
}
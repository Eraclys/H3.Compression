using System;
using System.Collections.Generic;
using System.Linq;

namespace H3.Compression
{
    public static class H3IndicesEncoder
    {
        public static byte[] Encode(this IReadOnlyCollection<ulong> data)
        {
            var ordered = Prepare(data);
           
            var resolution =  (byte)((data.First() & 0xF0000000000000L) >> 52);
            var bytes = new List<byte>{resolution};
            var shiftValue = (15 - resolution) * 3;
            var previousValue = ordered[0];
            var currentDelta =  previousValue >> shiftValue;
            var repeatCount = 1UL;

            for (var i = 1; i < ordered.Count; i++)
            {
                var value = ordered[i];
                var delta = value - previousValue;
                var compressedDelta = delta >> shiftValue;
                
                if (currentDelta == compressedDelta)
                {
                    repeatCount++;
                }
                else
                {
                    bytes.AddRange(RepeatedDeltaEncoder.Encode(new RepeatedDelta(currentDelta, repeatCount)));
                    currentDelta = compressedDelta;
                    repeatCount = 1;
                }
                
                previousValue = value;
            }

            bytes.AddRange(RepeatedDeltaEncoder.Encode(new RepeatedDelta(currentDelta, repeatCount)));
            
            return bytes.ToArray();
        }
        
        public static IEnumerable<ulong> Decode(this ReadOnlySpan<byte> bytes)
        {
            var results = new List<ulong>();
            var resolution = bytes[0];
            var shiftValue = (15 - resolution) * 3;
            var fillMask = ulong.MaxValue >> (64 - shiftValue);
            var previousValue = 0UL;
            bytes = bytes.Slice(1);

            while (!bytes.IsEmpty)
            {
                bytes = RepeatedDeltaEncoder.Decode(bytes, out var repeatedDelta);

                for (var i = 0UL; i < repeatedDelta.RepeatCount; i++)
                {
                    var delta = (repeatedDelta.Delta << shiftValue) + (results.Count == 0 ? fillMask: 0);
                    previousValue += delta;
                    results.Add(previousValue);
                }
            }

            return results;
        }

        static List<ulong> Prepare(IEnumerable<ulong> data) => data
            .OrderBy(x => x)
            .ToList();
    }
}
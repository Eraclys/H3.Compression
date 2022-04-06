using System;
using System.Collections.Generic;
using System.Linq;

namespace H3.Compression
{
    public static class H3IndicesEncoder
    {
        public static byte[] Encode(IEnumerable<ulong> data)
        {
            var ordered = Prepare(data);
           
            var resolution =  (byte)((ordered.First() & 0xF0000000000000L) >> 52);
            var bytes = new List<byte>(ordered.Count){resolution};
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
        
        public static IEnumerable<ulong> Decode(ReadOnlySpan<byte> bytes)
        {
            var resolution = bytes[0];
            var shiftValue = (15 - resolution) * 3;
            var fillMask = ulong.MaxValue >> (64 - shiftValue);
            var previousValue = 0UL;
            var results = new List<ulong>(bytes.Length);
            bytes = bytes.Slice(1);

            while (!bytes.IsEmpty)
            {
                bytes = RepeatedDeltaEncoder.Decode(bytes, out var repeatedDelta);
                var delta = (repeatedDelta.Delta << shiftValue) | fillMask;

                for (var i = 0UL; i < repeatedDelta.RepeatCount; i++)
                {
                    previousValue += delta;
                    results.Add(previousValue);
                }

                fillMask = 0;
            }

            return results;
        }

        static List<ulong> Prepare(IEnumerable<ulong> data) => data
            .OrderBy(x => x)
            .ToList();
    }
}
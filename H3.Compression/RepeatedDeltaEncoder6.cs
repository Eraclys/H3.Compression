using System;
using System.Collections.Generic;

namespace H3.Compression
{
    public static class RepeatedDeltaEncoder6
    {
        public static bool CanEncode(RepeatedDelta repeatedDelta) => repeatedDelta.Delta <= 0b00111111;

        public static byte[] Encode(RepeatedDelta repeatedDelta)
        {
            var results = new List<byte>();
            results.Add(ConstantEncoder6.Encode(repeatedDelta.Delta));
            results.AddRange(RepeatCountEncoder.Encode(repeatedDelta.RepeatCount - 1));
            return results.ToArray();
        }

        public static ReadOnlySpan<byte> Decode(ReadOnlySpan<byte> bytes, out RepeatedDelta repeatedDelta)
        {
            bytes = ConstantEncoder6.Decode(bytes, out var delta);
            bytes = RepeatCountEncoder.Decode(bytes, out var repeatCount);
            repeatedDelta = new RepeatedDelta(delta, repeatCount + 1);

            return bytes;
        }
    }
}
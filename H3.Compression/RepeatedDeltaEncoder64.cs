using System;
using System.Collections.Generic;

namespace H3.Compression
{
    public static class RepeatedDeltaEncoder64
    {
        public static byte[] Encode(RepeatedDelta repeatedDelta)
        {
            var results = new List<byte>();
            results.AddRange(ConstantEncoder64.Encode(repeatedDelta.Delta));
            results.AddRange(RepeatCountEncoder.Encode(repeatedDelta.RepeatCount - 1));
            return results.ToArray();
        }

        public static ReadOnlySpan<byte> Decode(ReadOnlySpan<byte> bytes, out RepeatedDelta repeatedDelta)
        {
            bytes = ConstantEncoder64.Decode(bytes, out var delta);
            bytes = RepeatCountEncoder.Decode(bytes, out var repeatCount);
            repeatedDelta = new RepeatedDelta(delta, repeatCount + 1);

            return bytes;
        }
    }
}
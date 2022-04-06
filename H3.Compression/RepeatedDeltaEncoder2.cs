using System;
using System.Collections.Generic;

namespace H3.Compression
{
    public static class RepeatedDeltaEncoder2
    {
        public static bool CanEncode(RepeatedDelta repeatedDelta) => repeatedDelta.Delta <= 0b11;

        public static byte[] Encode(RepeatedDelta repeatedDelta)
        {
            var results = new List<byte>();

            var maxValue = (ulong)0b1111;

            if (repeatedDelta.RepeatCount <= maxValue)
            {
                results.Add(ConstantEncoder2.Encode(repeatedDelta.Delta, repeatedDelta.RepeatCount));
                return results.ToArray();
            }

            results.Add(ConstantEncoder2.Encode(repeatedDelta.Delta, maxValue));
            results.AddRange(RepeatCountEncoder.Encode(repeatedDelta.RepeatCount - maxValue));

            return results.ToArray();
        }

        public static ReadOnlySpan<byte> Decode(ReadOnlySpan<byte> bytes, out RepeatedDelta repeatedDelta)
        {
            bytes = ConstantEncoder2.Decode(bytes, out var delta, out var repeatCount);
            bytes = RepeatCountEncoder.Decode(bytes, out var additionalRepeatCount);
            repeatedDelta = new RepeatedDelta(delta, repeatCount + additionalRepeatCount);

            return bytes;
        }
    }
}
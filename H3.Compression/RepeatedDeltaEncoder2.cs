using System;

namespace H3.Compression
{
    public static class RepeatedDeltaEncoder2
    {
        public static bool CanEncode(RepeatedDelta repeatedDelta) => repeatedDelta.Delta <= 0b11;

        public static byte[] Encode(RepeatedDelta repeatedDelta)
        {
            const ulong maxValue = 0b1111;

            if (repeatedDelta.RepeatCount <= maxValue)
                return new[] { ConstantEncoder2.Encode(repeatedDelta.Delta, repeatedDelta.RepeatCount) };

            var constant = ConstantEncoder2.Encode(repeatedDelta.Delta, maxValue);
            var repeatCount = RepeatCountEncoder.Encode(repeatedDelta.RepeatCount - maxValue);
            
            var results = new byte[repeatCount.Length + 1];
            results[0] = constant;
            Array.Copy(repeatCount, 0, results, 1, repeatCount.Length);
            return results;
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
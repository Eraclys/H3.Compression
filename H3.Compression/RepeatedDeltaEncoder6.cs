using System;

namespace H3.Compression
{
    public static class RepeatedDeltaEncoder6
    {
        public static bool CanEncode(RepeatedDelta repeatedDelta) => repeatedDelta.Delta <= 0b00111111;

        public static byte[] Encode(RepeatedDelta repeatedDelta)
        {
            var constant = ConstantEncoder6.Encode(repeatedDelta.Delta);
            var repeatCount = RepeatCountEncoder.Encode(repeatedDelta.RepeatCount - 1);
            
            var results = new byte[repeatCount.Length + 1];
            results[0] = constant;
            Array.Copy(repeatCount, 0, results, 1, repeatCount.Length);
            return results;
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
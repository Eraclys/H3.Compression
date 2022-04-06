using System;

namespace H3.Compression
{
    public static class RepeatedDeltaEncoder64
    {
        public static byte[] Encode(RepeatedDelta repeatedDelta)
        {
            var constant = ConstantEncoder64.Encode(repeatedDelta.Delta);
            var repeatCount = RepeatCountEncoder.Encode(repeatedDelta.RepeatCount - 1);

            var results = new byte[constant.Length + repeatCount.Length];
            Array.Copy(constant, results, constant.Length);
            Array.Copy(repeatCount, 0, results, constant.Length, repeatCount.Length);
            
            return results;
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
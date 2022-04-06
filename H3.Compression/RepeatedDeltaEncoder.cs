using System;

namespace H3.Compression
{
    public static class RepeatedDeltaEncoder
    {
        public static byte[] Encode(RepeatedDelta repeatedDelta)
        {
            if (RepeatedDeltaEncoder2.CanEncode(repeatedDelta))
                return RepeatedDeltaEncoder2.Encode(repeatedDelta);

            if (RepeatedDeltaEncoder6.CanEncode(repeatedDelta))
                return RepeatedDeltaEncoder6.Encode(repeatedDelta);

            return RepeatedDeltaEncoder64.Encode(repeatedDelta);
        }
        
        public static ReadOnlySpan<byte> Decode(ReadOnlySpan<byte> bytes, out RepeatedDelta repeatedDelta)
        {
            var type = TypeEncoder.Decode(bytes[0]);

            return type switch
            {
                0 => RepeatedDeltaEncoder2.Decode(bytes, out repeatedDelta),
                2 => RepeatedDeltaEncoder6.Decode(bytes, out repeatedDelta),
                3 => RepeatedDeltaEncoder64.Decode(bytes, out repeatedDelta),
                _ => throw new InvalidOperationException("Unexpected type")
            };
        }
    }
}
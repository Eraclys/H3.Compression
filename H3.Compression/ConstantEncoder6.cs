using System;

namespace H3.Compression
{
    public static class ConstantEncoder6
    {
        const byte RemoveHeaderMask = 0b00111111;
        
        public static byte Encode(ulong delta) => (byte)(0b10000000 | delta);

        public static ReadOnlySpan<byte> Decode(ReadOnlySpan<byte> bytes, out ulong delta)
        {
            var header = bytes[0];
            delta = (ulong)(header & RemoveHeaderMask);
            return bytes.Slice(1);
        }
    }
}
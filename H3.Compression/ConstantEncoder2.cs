using System;

namespace H3.Compression
{
    public static class ConstantEncoder2
    {
        const byte SelectRangeRepeatMask = 0b00001111;
        const byte SelectRangeDeltaMask = 0b00110000;
        
        public static byte Encode(ulong delta, ulong repeatCount) => (byte)((byte)(delta << 4) | repeatCount);
        
        public static ReadOnlySpan<byte> Decode(ReadOnlySpan<byte> bytes, out ulong delta, out ulong repeatCount)
        {
            var header = bytes[0];
            delta = (ulong)((header & SelectRangeDeltaMask) >> 4);
            repeatCount = (ulong)(header & SelectRangeRepeatMask);
            
            return bytes.Slice(1);
        }
    }
}
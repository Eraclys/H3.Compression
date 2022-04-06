using System;
using System.Collections.Generic;

namespace H3.Compression
{
    public static class RepeatCountEncoder
    {
        const byte RemoveHeaderMask = 0b00111111;
        
        public static byte[] Encode(ulong repeatCount)
        {
            var results = new List<byte>();
	
            while (repeatCount > 0)
            {
                results.Add((byte)(0b01000000 | (byte)(repeatCount % 63)));
                repeatCount /= 63;
            }
	
            return results.ToArray();
        }
        
        public static ReadOnlySpan<byte> Decode(ReadOnlySpan<byte> bytes, out ulong repeatCount)
        {
            repeatCount = 0;
            var multiplier = 1UL;
            
            while (bytes.Length > 0 && TypeEncoder.Decode(bytes[0]) == 1)
            {
                var value = (ulong)(bytes[0] & RemoveHeaderMask);
                repeatCount += value*multiplier;
                multiplier *= 63;
                bytes = bytes.Slice(1);
            }

            return bytes;
        }
    }
}
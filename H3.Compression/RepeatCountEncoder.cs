using System;

namespace H3.Compression
{
    public static class RepeatCountEncoder
    {
        const byte RemoveHeaderMask = 0b00111111;
        
        public static byte[] Encode(ulong repeatCount)
        {
            Span<byte> results = stackalloc byte[11];
            var index = 0;
            
            while (repeatCount > 0)
            {
                results[index++] = (byte)(0b01000000 | (byte)(repeatCount % 63));
                repeatCount /= 63;
            }
	
            return results.Slice(0, index).ToArray();
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
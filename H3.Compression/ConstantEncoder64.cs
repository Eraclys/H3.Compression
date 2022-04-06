using System;

namespace H3.Compression
{
    public static class ConstantEncoder64
    {     
        const byte RemoveHeaderMask = 0b00111111;
        
        public static byte[] Encode(ulong delta)
        {
            Span<byte> encoded = stackalloc byte[9];
            BitConverter.TryWriteBytes(encoded.Slice(1), delta);
            
            var byteCount = CountBytes(delta);
            encoded[0] = (byte)(0b11000000 | byteCount);
	
            return encoded.Slice(0, byteCount+1).ToArray();
        }

        public static ReadOnlySpan<byte> Decode(ReadOnlySpan<byte> bytes, out ulong delta)
        {
            var byteCount = bytes[0] & RemoveHeaderMask;

            Span<byte> numberBytes = stackalloc byte[8];
            
            bytes.Slice(1, byteCount).CopyTo(numberBytes);

            delta = BitConverter.ToUInt64(numberBytes);

            return bytes.Slice(1 + byteCount);
        }
        
        static int CountBytes(ulong n)
        {
            var count = 0;
	
            while (n != 0)
            {
                count++;
                n >>= 8;
            }

            return count;
        }
    }
}
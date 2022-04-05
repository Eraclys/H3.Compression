using System;
using System.Collections.Generic;

namespace H3.Compression
{
    public static class Decoder
    {
        public static IEnumerable<ulong> Decode(this ReadOnlySpan<byte> bytes, int resolution)
        {
            var results = new List<ulong>();
            var shiftValue = (15 - resolution) * 3;
            var fillMask = ulong.MaxValue >> (64 - shiftValue);
            var previousValue = 0UL;

            while (!bytes.IsEmpty)
            {
                bytes = DecodeVector(bytes, out var vector);

                for (var i = 0; i < vector.RepeatCount; i++)
                {
                    var delta = (vector.Delta << shiftValue) + (results.Count == 0 ? fillMask: 0);
                    previousValue += delta;
                    results.Add(previousValue);
                }
            }

            return results;
        }

        static ReadOnlySpan<byte> DecodeVector(ReadOnlySpan<byte> bytes, out Vector2 vector)
        {
            var type = DecodeType(bytes[0]);
            vector = new Vector2(0, 0);

            switch (type)
            {
                case 0: bytes = DecodeRange(bytes, out vector); break;
                case 2: bytes = DecodeSmallCompressedNumber(bytes, out vector); break;
                case 3: bytes = DecodeCompressedNumber(bytes, out vector); break;
                default: throw new InvalidOperationException("Unexpected type");
            }
    
            return bytes;
        }
        
        const byte HeaderMask = 0b00111111;
        const byte RangeRepeatMask = 0b00001111;
        const byte RangeDeltaMask = 0b00110000;

        static ReadOnlySpan<byte> DecodeSmallCompressedNumber(ReadOnlySpan<byte> bytes, out Vector2 vector)
        {
            var header = bytes[0];
            var delta = header & HeaderMask;
            vector = new Vector2((ulong)delta, 1);
            return bytes.Slice(1);
        }
        
        static ReadOnlySpan<byte> DecodeRange(ReadOnlySpan<byte> bytes, out Vector2 vector)
        {
            var header = bytes[0];
            var delta = (header & RangeDeltaMask) >> 4;
            var repeatCount = header & RangeRepeatMask;
            
            vector = new Vector2((ulong)delta, repeatCount);

            return bytes.Slice(1);
        }

        static ReadOnlySpan<byte> DecodeCompressedNumber(ReadOnlySpan<byte> bytes, out Vector2 vector)
        {
            var byteCount = bytes[0] & HeaderMask;

            Span<byte> numberBytes = stackalloc byte[8];
            
            bytes.Slice(1, byteCount).CopyTo(numberBytes);

            var delta = BitConverter.ToUInt64(numberBytes);

            vector = new Vector2(delta, 1);

            return bytes.Slice(1 + byteCount);
        }

        static int DecodeType(byte header) => header >> 6;
    }
}
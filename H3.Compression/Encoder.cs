using System;
using System.Collections.Generic;
using System.Linq;

namespace H3.Compression
{
    public static class Encoder
    {
        public static byte[] Encode(this IEnumerable<ulong> data, int resolution)
        {
            var deltas = CalculateDeltas(data, resolution);
            return ComputeByteArray(deltas);
        }

        static List<ulong> Prepare(IEnumerable<ulong> data) => data
            .OrderBy(x => x)
            .ToList();

        static List<Vector2> CalculateDeltas(this IEnumerable<ulong> data, int resolution)
        {	
            var ordered = Prepare(data);

            var results = new List<Vector2>();
            var shiftValue = (15 - resolution) * 3;
            var previousValue = ordered[0];
            var currentDelta =  previousValue >> shiftValue;
            var repeatCount = 1;

            for (var i = 1; i < ordered.Count; i++)
            {
                var value = ordered[i];
                var delta = value - previousValue;
                var compressedDelta = delta >> shiftValue;
                
                if (currentDelta == compressedDelta)
                {
                    repeatCount++;
                }
                else
                {
                    results.Add(new Vector2(currentDelta, repeatCount));
                    currentDelta = compressedDelta;
                    repeatCount = 1;
                }
                
                previousValue = value;
            }

            results.Add(new Vector2(currentDelta, repeatCount));
            
            return results;
        }
        
        static byte[] ComputeByteArray(List<Vector2> deltas)
        {
            var bytes = new List<byte>();
	
            foreach (var delta in deltas)
            {
                bytes.AddRange(Encode(delta.Delta, delta.RepeatCount));
            }
            
            return bytes.ToArray();
        }
        
        static IReadOnlyCollection<byte> Encode(ulong delta, int repeatCount)
        {
            var results = new List<byte>();

            // ranges
            if (delta <= 0b11)
            {
                if (repeatCount <= 0b1111)
                {
                    results.Add(ToRange(delta, repeatCount));
                    return results;
                }

                results.Add(ToRange(delta, 0b1111));
                results.AddRange(ToMultiplier(repeatCount - 0b1111));
                return results;
            }

            // short constant
            if (delta <= 0b00111111)
            {
                results.Add(ToSmallCompressedNumber(delta));
                results.AddRange(ToMultiplier(repeatCount - 1));
                return results;
            }

            // constant
            results.AddRange(ToCompressedNumber(delta));
            results.AddRange(ToMultiplier(repeatCount -1));
            return results;
        }
        
        static byte ToRange(ulong delta, int repeatCount) => (byte)((byte)(delta << 4) | repeatCount);
        static byte ToSmallCompressedNumber(ulong delta) => (byte)(0b10000000 | delta);

        static byte[] ToMultiplier(int multiplier)
        {
            var results = new List<byte>();
	
            while (multiplier > 0)
            {
                results.Add((byte)(0b01000000 | (byte)(multiplier % 63)));
                multiplier /= 63;
            }
	
            return results.ToArray();
        }

        static byte[] ToCompressedNumber(ulong delta)
        {
            var byteCount = CountBytes(delta);
            var header = (byte)(0b11000000 | byteCount);
            var deltaBytes = BitConverter.GetBytes(delta);

            var encoded = new byte[byteCount + 1];
            encoded[0] = header;
            Array.Copy(deltaBytes, 0, encoded, 1, byteCount);
	
            return encoded;
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
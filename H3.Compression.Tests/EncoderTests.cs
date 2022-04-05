using System;
using System.Globalization;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace H3.Compression.Tests
{
    public class EncoderTests
    {
        [Fact]
        public void ShouldEncode()
        {
            var indices = new[]
                {
                    "8a194ad36707fff",
                    "8a194ad3298ffff",
                    "8a194ad32c9ffff",
                    "8a195da4da17fff",
                    "8a194ad3670ffff",
                    "8a194ad36717fff",
                    "8a194ad3671ffff",
                    "8a194ad3299ffff"
                }
                .Select(x => ulong.Parse(x, NumberStyles.HexNumber))
                .ToArray();

            var encoded = indices.Encode(resolution: 10);

            Convert.ToBase64String(encoded).Should().Be("xjFlWilDESHBYMJNBxPEXy5aAg==");
            
            var decoded = Decoder.Decode(encoded, resolution: 10).ToList();

            decoded.Count.Should().Be(indices.Length);

            decoded.Should().BeEquivalentTo(indices.OrderBy(x => x));
        }
    }
}
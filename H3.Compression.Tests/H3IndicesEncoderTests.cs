using System.Globalization;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace H3.Compression.Tests
{
    public class H3IndicesEncoderTests
    {
        [Fact]
        public void ShouldEncode()
        {
            var indices = new[]
                {
                    "8a194ad32c9ffff",
                    "8a194ad3298ffff",
                    "8a194ad3299ffff",
                    "8a194ad3670ffff",
                    "8a194ad3671ffff",
                    "8a194ad36707fff",
                    "8a194ad36717fff",
                    "8a195da4da17fff",
                    "8A195DA4DC0FFFF",
                    "8A195DA4DE07FFF"
                }
                .Select(x => ulong.Parse(x, NumberStyles.HexNumber))
                .ToArray();

            var encoded = H3IndicesEncoder.Encode(indices);
            
            var decoded = H3IndicesEncoder.Decode(encoded).ToList();

            decoded.Count.Should().Be(indices.Length);

            decoded.Should().BeEquivalentTo(indices.OrderBy(x => x));
        }
    }
}
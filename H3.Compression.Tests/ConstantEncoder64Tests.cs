using FluentAssertions;
using Xunit;

namespace H3.Compression.Tests
{
    public class ConstantEncoder64Tests
    {
        [Theory]
        [InlineData(ulong.MinValue)]
        [InlineData(ulong.MaxValue / 2)]
        [InlineData(ulong.MaxValue)]
        public void ShouldEncodeAndDecode(ulong expectedDelta)
        {
            var encoded = ConstantEncoder64.Encode(expectedDelta);
            ConstantEncoder64.Decode(encoded, out var delta);
            
            delta.Should().Be(expectedDelta);
        }
    }
}
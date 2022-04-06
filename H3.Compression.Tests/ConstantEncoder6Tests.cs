using FluentAssertions;
using Xunit;

namespace H3.Compression.Tests
{
    public class ConstantEncoder6Tests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(34)]
        [InlineData(63)]
        public void ShouldEncodeAndDecode(ulong expectedDelta)
        {
            var encoded = ConstantEncoder6.Encode(expectedDelta);
            ConstantEncoder6.Decode(new [] {encoded}, out var delta);
            
            delta.Should().Be(expectedDelta);
        }
    }
}
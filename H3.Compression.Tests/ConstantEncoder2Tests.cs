using FluentAssertions;
using Xunit;

namespace H3.Compression.Tests
{
    public class ConstantEncoder2Tests
    {
        [Theory]
        [InlineData(0, 0)]
        [InlineData(1, 1)]
        [InlineData(2, 7)]
        [InlineData(3, 15)]
        public void ShouldEncodeAndDecode(ulong expectedDelta, ulong expectedRepeatCount)
        {
            var encoded = ConstantEncoder2.Encode(expectedDelta, expectedRepeatCount);
            ConstantEncoder2.Decode(new [] {encoded}, out var delta, out var repeatCount);
            
            delta.Should().Be(expectedDelta);
            repeatCount.Should().Be(expectedRepeatCount);
        }
    }
}
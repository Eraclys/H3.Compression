using FluentAssertions;
using Xunit;

namespace H3.Compression.Tests
{
    public class RepeatedDeltaEncoder2Tests
    {
        [Theory]
        [InlineData(ulong.MinValue, ulong.MinValue)]
        [InlineData(1, 15)]
        [InlineData(2, 5898)]
        [InlineData(3, ulong.MaxValue)]
        public void ShouldEncodeAndDecode(ulong expectedDelta, ulong expectedRepeatCount)
        {
            var expected = new RepeatedDelta(expectedDelta, expectedRepeatCount);
            
            var encoded = RepeatedDeltaEncoder2.Encode(expected);
            RepeatedDeltaEncoder2.Decode(encoded, out var actual);
            
            actual.Should().Be(expected);
        }
    }
}
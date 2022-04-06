using FluentAssertions;
using Xunit;

namespace H3.Compression.Tests
{
    public class RepeatedDeltaEncoder6Tests
    {
        [Theory]
        [InlineData(ulong.MinValue, ulong.MinValue)]
        [InlineData(1, 15)]
        [InlineData(34, 5898)]
        [InlineData(63, ulong.MaxValue)]
        public void ShouldEncodeAndDecode(ulong expectedDelta, ulong expectedRepeatCount)
        {
            var expected = new RepeatedDelta(expectedDelta, expectedRepeatCount);
            
            var encoded = RepeatedDeltaEncoder6.Encode(expected);
            RepeatedDeltaEncoder6.Decode(encoded, out var actual);
            
            actual.Should().Be(expected);
        }
    }
}
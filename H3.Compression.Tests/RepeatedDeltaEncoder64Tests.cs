using FluentAssertions;
using Xunit;

namespace H3.Compression.Tests
{
    public class RepeatedDeltaEncoder64Tests
    {
        [Theory]
        [InlineData(ulong.MinValue, ulong.MinValue)]
        [InlineData(ulong.MaxValue/2, ulong.MaxValue/2)]
        [InlineData(ulong.MaxValue, ulong.MaxValue)]
        public void ShouldEncodeAndDecode(ulong expectedDelta, ulong expectedRepeatCount)
        {
            var expected = new RepeatedDelta(expectedDelta, expectedRepeatCount);
            
            var encoded = RepeatedDeltaEncoder64.Encode(expected);
            RepeatedDeltaEncoder64.Decode(encoded, out var actual);
            
            actual.Should().Be(expected);
        }
    }
}
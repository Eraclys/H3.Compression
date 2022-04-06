using FluentAssertions;
using Xunit;

namespace H3.Compression.Tests
{
    public class RepeatCountEncoderTests
    {
        [Theory]
        [InlineData(ulong.MinValue)]
        [InlineData(ulong.MaxValue / 2)]
        [InlineData(ulong.MaxValue)]
        public void ShouldEncodeAndDecode(ulong expectedRepeatCount)
        {
            var encoded = RepeatCountEncoder.Encode(expectedRepeatCount);
            RepeatCountEncoder.Decode(encoded, out var repeatCount);
            
            repeatCount.Should().Be(expectedRepeatCount);
        }
    }
}
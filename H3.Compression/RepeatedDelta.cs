namespace H3.Compression
{
    public readonly struct RepeatedDelta
    {
        public readonly ulong Delta;
        public readonly ulong RepeatCount;

        public RepeatedDelta(ulong delta, ulong repeatCount)
        {
            Delta = delta;
            RepeatCount = repeatCount;
        }
    }
}
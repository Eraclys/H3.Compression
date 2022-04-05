namespace H3.Compression
{
    readonly struct Vector2
    {
        public readonly ulong Delta;
        public readonly int RepeatCount;

        public Vector2(ulong delta, int repeatCount)
        {
            Delta = delta;
            RepeatCount = repeatCount;
        }
    }
}
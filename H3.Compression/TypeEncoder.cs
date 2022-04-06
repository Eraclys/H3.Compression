namespace H3.Compression
{
    public static class TypeEncoder
    {
        public static int Decode(byte value) => value >> 6;
    }
}

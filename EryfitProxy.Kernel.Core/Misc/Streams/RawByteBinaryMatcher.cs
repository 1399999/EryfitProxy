

using System;

namespace EryfitProxy.Kernel.Misc.Streams
{
    public class RawByteBinaryMatcher : IBinaryMatcher
    {
        public BinaryMatchResult FindIndex(ReadOnlySpan<byte> buffer, ReadOnlySpan<byte> searchText)
        {
            return new BinaryMatchResult(buffer.IndexOf(searchText), searchText.Length, searchText.Length);
        }
    }
}

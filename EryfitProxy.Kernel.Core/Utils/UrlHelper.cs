

using System;

namespace EryfitProxy.Kernel.Utils
{
    internal static class UrlHelper
    {
        public static bool IsAbsoluteHttpUrl(ReadOnlySpan<char> rawUrl)
        {
            return rawUrl.StartsWith("http://".AsSpan(), StringComparison.OrdinalIgnoreCase) ||
                   rawUrl.StartsWith("https://".AsSpan(), StringComparison.OrdinalIgnoreCase);
        }
    }
}

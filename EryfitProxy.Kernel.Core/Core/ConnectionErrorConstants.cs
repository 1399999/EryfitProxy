

namespace EryfitProxy.Kernel.Core
{
    internal static class ConnectionErrorConstants
    {
        public static readonly string Generic502 =
            "HTTP/1.1 528 Eryfit error\r\n" +
            "x-eryfit: Eryfit error\r\n" +
            "Content-length: {0}\r\n" +
            "Content-type: text/plain\r\n" +
            "Connection : close\r\n\r\n";
    }
}

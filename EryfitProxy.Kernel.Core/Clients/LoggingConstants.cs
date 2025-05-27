

using System;
using System.IO;

namespace EryfitProxy.Kernel.Clients
{
    internal static class LoggingConstants
    {
        public static string DefaultTracingDirectory { get; } = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            ".eryfit", "logs", "debug");
    }
}

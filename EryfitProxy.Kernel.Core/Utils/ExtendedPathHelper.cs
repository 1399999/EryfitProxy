

using System;
using System.IO;
using System.Runtime.InteropServices;

namespace EryfitProxy.Kernel.Utils
{
    // On linux and OSX, .NET returns /tmp/ directory which requires elevation to write to.
    // Instead we use %appdata%/.eryfit/temp directory with %appdata% a custom environment variable
    // specific to eryfit
    internal static class ExtendedPathHelper
    {
        public static string GetTempPath()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return Path.GetTempPath();
            
            
            var fullPath = Environment.ExpandEnvironmentVariables("%appdata%/.eryfit/temp");
            Directory.CreateDirectory(fullPath);

            return fullPath; 
        }

        public static string GetTempFileName()
        {
            var tempPath = GetTempPath(); 
            
            var fileName = Path.GetRandomFileName();
            
            return Path.Combine(tempPath, fileName);
        }
    }
}

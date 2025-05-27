

using System.Reflection;

namespace EryfitProxy.Kernel
{
    internal static class EryfitMetaInformation
    {
        private static string? _version;

        public static string? Version {
            get
            {
                if (_version != null)
                    return _version;

                return _version = typeof(Proxy).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
                    .InformationalVersion ?? "1.0.0";
            }
        }
    }
}

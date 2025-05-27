

using EryfitProxy.Kernel.Core;

namespace EryfitProxy.Kernel.Certificates
{
    internal class EryfitSecurityParams
    {
        public static EryfitSecurity Current { get; } = new EryfitSecurity(EryfitSecurity.DefaultCertificatePath, new SystemEnvironmentProvider());

    }
}

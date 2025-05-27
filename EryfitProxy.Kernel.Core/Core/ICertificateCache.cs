

using System;

namespace EryfitProxy.Kernel.Core
{
    public interface ICertificateCache
    {
        byte[] Load(string baseCertificateSerialNumber, string rootDomain, Func<string, byte[]> certificateBuilder);
    }
}

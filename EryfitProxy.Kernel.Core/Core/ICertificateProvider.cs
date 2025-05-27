

using System;
using System.Security.Cryptography.X509Certificates;

namespace EryfitProxy.Kernel.Core
{
    /// <summary>
    /// A certificate provider for Eryfit
    /// </summary>
    public interface ICertificateProvider : IDisposable
    {
        /// <summary>
        /// Retrieve a certificate for a particular host 
        /// </summary>
        /// <param name="hostName"></param>
        /// <returns></returns>
        X509Certificate2 GetCertificate(string hostName);
    }
}

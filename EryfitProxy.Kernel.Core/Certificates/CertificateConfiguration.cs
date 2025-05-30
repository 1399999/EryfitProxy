

namespace EryfitProxy.Kernel.Certificates
{
    /// <summary>
    /// Defines how to retrieve a certificate
    /// </summary>
    public enum CertificateRetrieveMode
    {
        /// <summary>
        /// The default built-in certificate
        /// </summary>
        EryfitDefault,

        /// <summary>
        /// From the current store, by serial number
        /// </summary>
        FromUserStoreSerialNumber,

        /// <summary>
        /// From the current store, by thumbprint
        /// </summary>
        FromUserStoreThumbPrint,

        /// <summary>
        ///     Pfx and p12 files
        /// </summary>
        FromPkcs12
    }
}

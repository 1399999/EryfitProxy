

using EryfitProxy.Kernel.Clients.H2;

namespace EryfitProxy.Kernel.Clients.Ssl
{
    public class AdvancedTlsSettings
    {
        /// <summary>
        /// TLS fingerprint settings
        /// </summary>
        public TlsFingerPrint? TlsFingerPrint { get; set; }

        /// <summary>
        /// H2 stream settings
        /// </summary>
        public H2StreamSetting ? H2StreamSetting { get; set; }
    }
}

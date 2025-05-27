

using EryfitProxy.Kernel.Clients;

namespace EryfitProxy.Kernel.Core
{
    internal static class ExchangeSourceProviderHelper
    {
        public static ExchangeSourceProvider GetSourceProvider(EryfitSetting setting,
            SecureConnectionUpdater secureConnectionUpdater, 
            IIdProvider idProvider, ICertificateProvider certificateProvider, 
            ProxyAuthenticationMethod proxyAuthenticationMethod, IExchangeContextBuilder contextBuilder)
        {
            if (!setting.ReverseMode)
                return new FromProxyConnectSourceProvider(
                    secureConnectionUpdater, idProvider,
                    proxyAuthenticationMethod, contextBuilder);
            
            if (setting.ReverseModePlainHttp)
                return new ReverseProxyPlainExchangeSourceProvider(idProvider, setting.ReverseModeForcedPort, contextBuilder);

            return new ReverseProxyExchangeSourceProvider(certificateProvider, idProvider, setting.ReverseModeForcedPort, contextBuilder);
        }
    }
}

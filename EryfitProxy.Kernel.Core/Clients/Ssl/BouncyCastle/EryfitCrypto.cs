

using Org.BouncyCastle.Security;
using Org.BouncyCastle.Tls;
using Org.BouncyCastle.Tls.Crypto;
using Org.BouncyCastle.Tls.Crypto.Impl.BC;

namespace EryfitProxy.Kernel.Clients.Ssl.BouncyCastle
{
    internal class EryfitCrypto : BcTlsCrypto
    {
        public TlsClientContext? Context { get; private set; }

        public EryfitCrypto() : base(new SecureRandom())
        {
        }

        public byte[]? MasterSecret { get; set; }


        public override TlsSecret AdoptSecret(TlsSecret secret)
        {
            var resultSecret = base.AdoptSecret(secret);

            MasterSecret = secret.ExtractKeySilently();

            return resultSecret;
        }

        public void UpdateContext(TlsClientContext context)
        {
            Context = context;
        }
    }
}

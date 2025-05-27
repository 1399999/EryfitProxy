

using Org.BouncyCastle.Tls.Crypto;

namespace EryfitProxy.Kernel.Clients.Ssl.BouncyCastle
{
    internal static class SecretKeyExtractor
    {
        private static readonly EncryptInPlain Encryptor = new();

        public static byte[] ExtractKeySilently(this TlsSecret secret)
        {
            return secret.Encrypt(Encryptor);
        }
    }
}

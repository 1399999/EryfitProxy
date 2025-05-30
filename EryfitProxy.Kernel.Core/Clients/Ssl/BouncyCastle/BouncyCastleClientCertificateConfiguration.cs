

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EryfitProxy.Kernel.Core;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Tls;
using Org.BouncyCastle.Tls.Crypto;
using Org.BouncyCastle.Tls.Crypto.Impl.BC;

namespace EryfitProxy.Kernel.Clients.Ssl.BouncyCastle
{
    internal class BouncyCastleClientCertificateConfiguration
    {
        public BouncyCastleClientCertificateConfiguration(Certificate certificate, AsymmetricKeyParameter privateKey)
        {
            Certificate = certificate;
            PrivateKey = privateKey;
        }

        public Certificate Certificate { get; }

        public AsymmetricKeyParameter PrivateKey { get; }

        public static BouncyCastleClientCertificateConfiguration CreateFrom(
            CertificateRequest certificateRequest,
            EryfitCrypto eryfitCrypto, BouncyCastleClientCertificateInfo info)
        {
            var store = new Pkcs12StoreBuilder().Build();

            using var stream = File.OpenRead(info.Pkcs12File);

            store.Load(stream, info.Pkcs12Password?.ToCharArray() ?? Array.Empty<char>());

            var mainCert = store.Aliases.First();

            var bclTlsCertificates = new List<BcTlsCertificate>();

            foreach (var chain  in store.GetCertificateChain(mainCert)) {
                
                var tlsCertificate = new BcTlsCertificate(eryfitCrypto, chain.Certificate.CertificateStructure);

                bclTlsCertificates.Add(tlsCertificate);
            }

            var certificate = new Certificate(
               certificateRequest.GetCertificateRequestContext(),
               bclTlsCertificates.Select(s => new CertificateEntry(s, null)).ToArray());

             var keyEntry = store.GetKey(mainCert);

            var rsaKeyParameters = (RsaKeyParameters) keyEntry.Key;

            return new BouncyCastleClientCertificateConfiguration(certificate, rsaKeyParameters);
        }
    }

    public class BouncyCastleClientCertificateInfo
    {
        public BouncyCastleClientCertificateInfo(string pkcs12File, string? pkcs12Password = null)
        {
            Pkcs12File = pkcs12File;
            Pkcs12Password = pkcs12Password;
        }

        public string Pkcs12File { get;  }

        public string? Pkcs12Password{ get;  }
    }
}

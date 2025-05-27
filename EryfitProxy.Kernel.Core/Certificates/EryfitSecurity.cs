

using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using EryfitProxy.Kernel.Core;
using EryfitProxy.Kernel.Core;

namespace EryfitProxy.Kernel.Certificates
{
    /// <summary>
    ///  Solve the default root certificate used by eryfit in the following order:
    ///  - From the path specified in the environment variable ERYFIT_ROOT_CERTIFICATE which must be a path to a PKCS12 File
    ///  - From the static filesystem path %appdata%/.eryfit/rootca.pfx
    ///  - If none of the above is available, use the built-in certificate
    ///
    ///  For the two first cases, if the PKCS12 file has a password, it must be specified in the environment variable ERYFIT_ROOT_CERTIFICATE_PASSWORD
    /// </summary>
    internal class EryfitSecurity
    {
        public static readonly string DefaultCertificatePath = "%appdata%/.eryfit/rootca.pfx";
        private readonly string _certificatePath;
        private readonly EnvironmentProvider _environmentProvider;

        public EryfitSecurity(string certificatePath, EnvironmentProvider environmentProvider)
        {
            _certificatePath = certificatePath;
            _environmentProvider = environmentProvider;
            BuiltinCertificate = GetDefaultCertificate();

            if (!BuiltinCertificate.HasPrivateKey)
            {
                throw new ArgumentException("The built-in certificate must have a private key");
            }
        }

        public X509Certificate2 BuiltinCertificate { get; }

        private X509Certificate2 GetDefaultCertificate()
        {
            var certificatePath = _environmentProvider.GetEnvironmentVariable("ERYFIT_ROOT_CERTIFICATE");
            var certificatePassword = _environmentProvider.GetEnvironmentVariable("ERYFIT_ROOT_CERTIFICATE_PASSWORD");

            if (certificatePath != null) {
                if (!File.Exists(certificatePath)) {
                    throw new Exception($"The certificate file {certificatePath} (from ERYFIT_ROOT_CERTIFICATE variable) does not exist");
                }
            }
            else
            {
                var defaultPath = _environmentProvider.ExpandEnvironmentVariables(_certificatePath);

                if (File.Exists(defaultPath))
                {
                    certificatePath = defaultPath;
                }
            }

            if (certificatePath != null)
            {
                if (certificatePassword == null)
                {
                    return new X509Certificate2(certificatePath);
                }

                return new X509Certificate2(certificatePath, certificatePassword);
            }

            return new X509Certificate2(FileStore.Eryfit, "youshallnotpass");
        }

        public static void SetDefaultCertificateForUser(
            byte[] certificateContent,
            EnvironmentProvider environmentProvider, 
            string certificatePath)
        {
            var certificateFileInfo = new FileInfo(environmentProvider.ExpandEnvironmentVariables(certificatePath));
            var certificateDirectory = certificateFileInfo.Directory;

            if (certificateDirectory != null) {
                Directory.CreateDirectory(certificateDirectory.FullName);
            }

            File.WriteAllBytes(certificateFileInfo.FullName, certificateContent);
        }
    }
}

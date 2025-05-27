

using System.IO;
using System.Security.Cryptography.X509Certificates;
using EryfitProxy.Kernel.Misc;
using EryfitProxy.Kernel.Utils;

namespace EryfitProxy.Kernel.Certificates
{
    /// <summary>
    ///    Extended certificate installer for MacOs
    /// </summary>
    public static class ExtendedMacOsCertificateInstaller
    {
        /// <summary>
        /// Check if certificate is installed with : security verify-cert -c
        /// </summary>
        /// <param name="certificate"></param>
        /// <param name="tryElevate"></param>
        /// <returns></returns>
        public static bool IsCertificateInstalled(X509Certificate2 certificate, bool tryElevate)
        {
            var tempFile = ExtendedPathHelper.GetTempFileName();

            try {

                certificate.ExportToPem(tempFile);
                
                var runResult = ProcessUtils.QuickRun("security", $"verify-cert -c \"{tempFile}\"");
                return runResult.ExitCode == 0;
            }
            finally {
                if (File.Exists(tempFile))
                    File.Delete(tempFile);
            }
        }

        /// <summary>
        /// Install certificate with : security add-trusted-cert -d -r trustRoot -k /Library/Keychains/System.keychain r.cer
        /// </summary>
        /// <param name="certificate"></param>
        /// <param name="tryElevate"></param>
        /// <returns></returns>
        public static bool Install(X509Certificate2 certificate, bool tryElevate)
        {
            var tempFile = ExtendedPathHelper.GetTempFileName();

            try {
                certificate.ExportToPem(tempFile);

                if (tryElevate) {
                    var res = ProcessUtils.RunElevated("security",
                                               new[] {
                            "add-trusted-cert", "-d", "-r", "trustRoot", "-k",
                            "/Library/Keychains/System.keychain", tempFile
                        }, false, "");

                    if (res == null)
                        return false;

                    res.WaitForExit();

                    return res.ExitCode == 0;
                }

                var runResult = ProcessUtils.QuickRun("security",
                    $"add-trusted-cert -d -r trustRoot -k /Library/Keychains/System.keychain \"{tempFile}\"");

                return runResult.ExitCode == 0;
            }
            finally {
                if (File.Exists(tempFile))
                    File.Delete(tempFile);
            }
        }
    }
}

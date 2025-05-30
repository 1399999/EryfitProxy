namespace EryfitProxy.Kernel.Cli.System
{
    /// <summary>
    ///     This class provides features to allow running Eryfit sudo operation in a separate process
    /// </summary>
    public class OutOfProcAuthorityManager : DefaultCertificateAuthorityManager
    {
        private readonly string _currentBinaryFullPath;

        public OutOfProcAuthorityManager()
        {
            _currentBinaryFullPath = new FileInfo(Assembly.GetExecutingAssembly().Location).FullName;
        }

        public override async ValueTask<bool> RemoveCertificate(string thumbPrint)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) {
                await ProcessUtils.QuickRunAsync($"{_currentBinaryFullPath.RemoveFileExtension()}",
                    $" cert uninstall {thumbPrint}");
                
                // We are using pkexec for linux 
                var result = await ProcessUtils.QuickRunAsync("pkexec",
                    $"{_currentBinaryFullPath.RemoveFileExtension()} cert uninstall {thumbPrint}");

                return result.ExitCode == 0;
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
                // We are using runas for windows 

                var result = await ProcessUtils.QuickRunAsync("runas",
                    $"/user:Administrator uninstall {thumbPrint}");

                return result.ExitCode == 0;
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) {
                var result = await ProcessUtils.QuickRunAsync("pkexec",
                    $"{_currentBinaryFullPath.RemoveFileExtension()} cert uninstall {thumbPrint}");

                return result.ExitCode == 0;
            }

            throw new PlatformNotSupportedException();
        }

        public override async ValueTask<bool> InstallCertificate(X509Certificate2 certificate)
        {
            var buffer = new byte [8 * 1024];
            var memoryStream = new MemoryStream(buffer);

            certificate.ExportToPem(memoryStream);

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) {
                // installing certificate for current user

                // Working script for fedora
                // cp yo.pem /etc/pki/ca-trust/source/anchors/yo.pem
                // update-ca-trust 
                // /usr/local/share/ca-certificates --> ubuntu (must be crt extension) 
                // update-ca-certificates

                // Install for current user 

                await ProcessUtils.QuickRunAsync(
                    $"{_currentBinaryFullPath.RemoveFileExtension()}", "cert install",
                    new MemoryStream(buffer, 0, (int) memoryStream.Position));

                // Install for root 

                var result = await ProcessUtils.QuickRunAsync("pkexec",
                    $"\"{_currentBinaryFullPath.RemoveFileExtension()}\" cert install",
                    new MemoryStream(buffer, 0, (int) memoryStream.Position));

                return result.ExitCode == 0;
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
                // We are using runas for windows 

                throw new NotSupportedException("Out of proc installation is not supported on windows."); 
            }

            // Adding root certificate on macos s
            // sudo security add-trusted-cert -d -r trustRoot -k /Library/Keychains/System.keychain r.cer 

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) {
                
                ExtendedMacOsCertificateInstaller.Install(certificate, false);

                return true; 
            }

            throw new PlatformNotSupportedException();
        }
    }

    internal static class RemoveFileExtensions
    {
        public static string RemoveFileExtension(this string fileName)
        {
            // remove file extension and return 

            var tab = fileName.Split(".");

            return string.Join(".", tab[..^1]);
        }
    }


}

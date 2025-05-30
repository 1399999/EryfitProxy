

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using EryfitProxy.Kernel.Misc;

namespace EryfitProxy.Kernel.Utils.NativeOps.SystemProxySetup.macOs
{
    internal class MacOsHelper
    {
        // Adding root certificate on macos s

        public static async Task<IEnumerable<NetworkInterface>> GetEnabledInterfaces()
        {
            var runResult = await ProcessUtils.QuickRunAsync("networksetup", "-listnetworkserviceorder");

            if (runResult.ExitCode != 0 || runResult.StandardOutputMessage == null)
                throw new InvalidOperationException("Failed to get interfaces");

            var commandResponse = runResult.StandardOutputMessage;

            return ParseInterfaces(commandResponse);
        }

        public static IEnumerable<NetworkInterface> ParseInterfaces(string commandResponse)
        {
            var networkInterfaces = System.Net.NetworkInformation.NetworkInterface
                                          .GetAllNetworkInterfaces()
                                          .Where(n => n.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                                          .Where(n => n.NetworkInterfaceType != NetworkInterfaceType.Unknown)
                                          .Where(n => n.OperationalStatus == OperationalStatus.Up)
                                          .Where(n => n.GetIPProperties().UnicastAddresses.Any())
                                          .ToList();
            
            var hardwarePorMapping = NetworkInterface.ParseHardwarePortMapping(
                commandResponse.Split(new[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries));

            return networkInterfaces.Select(s => !hardwarePorMapping.TryGetValue(s.Name, out var hardwarePort) ?
                null : 
                new NetworkInterface(s.Name, s.Name, hardwarePort))
                                    .OfType<NetworkInterface>();
        }

        public static async Task<Dictionary<string, NetworkInterfaceProxySetting?>> ReadProxySettings(IEnumerable<string> interfaceNames)
        {
            var result = new Dictionary<string, NetworkInterfaceProxySetting?>();

            foreach (var interfaceName in interfaceNames) {

                var getwebProxyResult = await ProcessUtils.QuickRunAsync("networksetup", $"-getsecurewebproxy \"{interfaceName}\"");

                if (getwebProxyResult.ExitCode != 0 || getwebProxyResult.StandardOutputMessage == null)
                    continue;

                var commandResponse = getwebProxyResult.StandardOutputMessage;

                var proxySetting = NetworkInterfaceProxySetting.ParseFromCommandLineResult(commandResponse);

                result[interfaceName] = proxySetting;

                if (proxySetting != null) {
                    // Proxy settigns is available we try to set bypass domains

                    var byPassDomainsRunResult = await ProcessUtils.QuickRunAsync("networksetup", $"-getproxybypassdomains \"{interfaceName}\"");

                    if (byPassDomainsRunResult.ExitCode != 0 || byPassDomainsRunResult.StandardOutputMessage == null)
                        continue;

                    var byPassDomains = byPassDomainsRunResult.StandardOutputMessage.Split(new[] { "\r", "\n" },
                        StringSplitOptions.RemoveEmptyEntries).Distinct().ToArray();

                    proxySetting.ByPassDomains = byPassDomains;
                }
            }

            return result;
        }
    }
}

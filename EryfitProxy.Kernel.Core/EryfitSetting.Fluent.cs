

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Authentication;
using EryfitProxy.Kernel.Certificates;
using EryfitProxy.Kernel.Rules;
using EryfitProxy.Kernel.Rules.Actions;
using EryfitProxy.Kernel.Rules.Extensions;
using EryfitProxy.Kernel.Rules.Filters;
using EryfitProxy.Kernel.Rules.Filters.RequestFilters;
using Action = EryfitProxy.Kernel.Rules.Action;

#pragma warning disable CS0618 // Type or member is obsolete

namespace EryfitProxy.Kernel
{
    public partial class EryfitSetting
    {
        public EryfitSetting SetSaveFilter(Filter saveFilter)
        {
            SaveFilter = saveFilter;

            return this;
        }

        public EryfitSetting ClearSaveFilter()
        {
            SaveFilter = null;

            return this;
        }

        /// <summary>
        ///     Set hosts that bypass the proxy
        /// </summary>
        /// <param name="hosts"></param>
        /// <returns></returns>
        public EryfitSetting SetByPassedHosts(params string[] hosts)
        {
            ByPassHostFlat = string.Join(";", hosts.Distinct());

            return this;
        }

        /// <summary>
        ///     Set archiving policy
        /// </summary>
        /// <param name="archivingPolicy"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public EryfitSetting SetArchivingPolicy(ArchivingPolicy archivingPolicy)
        {
            ArchivingPolicy = archivingPolicy ?? throw new ArgumentNullException(nameof(archivingPolicy));

            return this;
        }

        public EryfitSetting SetOutDirectory(string directoryName)
        {
            ArchivingPolicy = ArchivingPolicy.CreateFromDirectory(directoryName);

            return this;
        }

        /// <summary>
        ///     Avoid certificate validation
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public EryfitSetting SetSkipRemoteCertificateValidation(bool value)
        {
            SkipRemoteCertificateValidation = value;

            return this;
        }

        /// <summary>
        ///     Add hosts that eryfit should not decrypt
        /// </summary>
        /// <param name="hosts"></param>
        /// <returns></returns>
        public EryfitSetting AddTunneledHosts(params string[] hosts)
        {
            foreach (var host in hosts.Where(h => !string.IsNullOrWhiteSpace(h))) {
                InternalAlterationRules.Add(new Rule(
                    new SkipSslTunnelingAction(),
                    new HostFilter(host, StringSelectorOperation.Exact)));
            }

            return this;
        }

        /// <summary>
        ///     Clear all bound addresses
        /// </summary>
        /// <returns></returns>
        public EryfitSetting ClearBoundAddresses()
        {
            BoundPoints.Clear();

            return this;
        }

        /// <summary>
        ///     Append a new endpoint to the bound address list
        /// </summary>
        /// <param name="endpoint">The IP address and port to listen to</param>
        /// <param name="default">
        ///     If this Endpoint is the default endpoint. When true, the automatic system proxy address will
        ///     prioritize this endpoint
        /// </param>
        /// <returns></returns>
        public EryfitSetting AddBoundAddress(IPEndPoint endpoint, bool? @default = null)
        {
            var isDefault = @default ?? BoundPoints.All(e => !e.Default);
            BoundPoints.Add(new ProxyBindPoint(endpoint, isDefault));

            return this;
        }

        /// <summary>
        ///     Append a new endpoint to the bound address list
        /// </summary>
        /// <param name="boundAddress">Valid IPv4 or IPv6 address to listen to</param>
        /// <param name="port">Port number to listen to</param>
        /// <param name="default">
        ///     If this Endpoint is the default endpoint. When true, the automatic system proxy address will
        ///     prioritize this endpoint
        /// </param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public EryfitSetting AddBoundAddress(string boundAddress, int port, bool? @default = null)
        {
            if (!IPAddress.TryParse(boundAddress, out var address)) {
                throw new ArgumentException($"{boundAddress} is not a valid IP address");
            }

            if (port < 0 || port >= ushort.MaxValue) {
                throw new ArgumentOutOfRangeException(nameof(port), $"port should be between 1 and {ushort.MaxValue}");
            }

            return AddBoundAddress(new IPEndPoint(address, port), @default);
        }

        /// <summary>
        ///     Append a new endpoint to the bound address list
        /// </summary>
        /// <param name="boundAddress">Valid IPv4 or IPv6 address to listen to. This property accepts IpAddress.Any (0.0.0.0).</param>
        /// <param name="port">Port number to listen to</param>
        /// <param name="default">
        ///     If this Endpoint is the default endpoint. When true, the automatic system proxy address will
        ///     prioritize this endpoint
        /// </param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public EryfitSetting AddBoundAddress(IPAddress boundAddress, int port, bool? @default = null)
        {
            if (port < 0 || port >= ushort.MaxValue) {
                throw new ArgumentOutOfRangeException(nameof(port), $"port should be between 1 and {ushort.MaxValue}");
            }

            return AddBoundAddress(new IPEndPoint(boundAddress, port), @default);
        }

        /// <summary>
        ///     Clear all bound addresses and set a new one
        /// </summary>
        /// <param name="boundAddress"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public EryfitSetting SetBoundAddress(string boundAddress, int port)
        {
            if (!IPAddress.TryParse(boundAddress, out var address)) {
                throw new ArgumentException($"{boundAddress} is not a valid IP address");
            }

            if (port < 0 || port >= ushort.MaxValue) {
                throw new ArgumentOutOfRangeException(nameof(port), $"port should be between 1 and {ushort.MaxValue}");
            }

            BoundPoints.Clear();
            BoundPoints.Add(new ProxyBindPoint(new IPEndPoint(address, port), true));

            return this;
        }

        /// <summary>
        ///     Clear all bound addresses and set a new one
        /// </summary>
        /// <param name="boundAddress">Valid IPv4 or IPv6 address to listen to. This property accepts IpAddress.Any (0.0.0.0).</param>
        /// <param name="port">Port number to listen to</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public EryfitSetting SetBoundAddress(IPAddress boundAddress, int port)
        {
            if (port < 0 || port >= ushort.MaxValue) {
                throw new ArgumentOutOfRangeException(nameof(port), $"port should be between 1 and {ushort.MaxValue}");
            }

            BoundPoints.Clear();
            BoundPoints.Add(new ProxyBindPoint(new IPEndPoint(boundAddress, port), true));

            return this;
        }

        /// <summary>
        ///     Set the number of concurrent connection per host maintained by the connection pool excluding websocket connections.
        ///     This option is ignored for H2 remote connection
        /// </summary>
        /// <param name="connectionPerHost"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public EryfitSetting SetConnectionPerHost(int connectionPerHost)
        {
            if (connectionPerHost < 1 || connectionPerHost >= 64) {
                throw new ArgumentOutOfRangeException(nameof(connectionPerHost), "value should be between 1 and 64");
            }

            ConnectionPerHost = connectionPerHost;

            return this;
        }

        /// <summary>
        ///     Set the default protocols used by eryfit
        /// </summary>
        /// <param name="protocols"></param>
        /// <returns></returns>
        public EryfitSetting SetServerProtocols(SslProtocols protocols)
        {
            ServerProtocols = protocols;

            return this;
        }

        /// <summary>
        ///     If false, eryfit will not check the revocation status of remote certificates.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public EryfitSetting SetCheckCertificateRevocation(bool value)
        {
            CheckCertificateRevocation = value;

            return this;
        }

        /// <summary>
        ///     If true, eryfit will automatically install the certificate in the user store.
        ///     This call needs administrator/root privileges.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public EryfitSetting SetAutoInstallCertificate(bool value)
        {
            AutoInstallCertificate = value;

            return this;
        }

        /// <summary>
        ///     Skip global ssl decryption. Eryfit will performs only blind ssl tunneling.
        ///     This option may disables several filters and actions based on clear text traffic.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public EryfitSetting SetSkipGlobalSslDecryption(bool value)
        {
            GlobalSkipSslDecryption = value;

            return this;
        }

        /// <summary>
        ///     Change the default certificate used by eryfit
        /// </summary>
        /// <returns></returns>
        public EryfitSetting SetCaCertificate(Certificate caCertificate)
        {
            CaCertificate = caCertificate;

            return this;
        }

        public EryfitSetting SetDisableCertificateCache(bool value)
        {
            DisableCertificateCache = value;

            return this;
        }

        /// <summary>
        ///     Remove existing alteration rules
        /// </summary>
        /// <returns></returns>
        public EryfitSetting ClearAlterationRules()
        {
            InternalAlterationRules.Clear();

            return this;
        }

        /// <summary>
        ///     Add alteration rules
        /// </summary>
        /// <param name="rules"></param>
        /// <returns></returns>
        public EryfitSetting AddAlterationRules(params Rule[] rules)
        {
            InternalAlterationRules.AddRange(rules);

            return this;
        }

        /// <summary>
        ///     Add alteration rules
        /// </summary>
        /// <param name="rules"></param>
        /// <returns></returns>
        public EryfitSetting AddAlterationRules(IEnumerable<Rule> rules)
        {
            InternalAlterationRules.AddRange(rules);

            return this;
        }

        /// <summary>
        ///     Add alteration rules from a filter and an action
        /// </summary>
        /// <param name="action"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public EryfitSetting AddAlterationRules(Action action, Filter filter)
        {
            InternalAlterationRules.Add(new Rule(action, filter));

            return this;
        }

        /// <summary>
        ///     Add alteration rules for any requests
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public EryfitSetting AddAlterationRulesForAny(Action action)
        {
            InternalAlterationRules.Add(new Rule(action, AnyFilter.Default));

            return this;
        }

        /// <summary>
        ///     Add alteration rules from a config file
        /// </summary>
        /// <param name="plainConfiguration"></param>
        /// <returns></returns>
        public EryfitSetting AddAlterationRules(string plainConfiguration)
        {
            var parser = new RuleConfigParser();

            var ruleSet = parser.TryGetRuleSetFromYaml(plainConfiguration, out var readErrors);

            if (readErrors != null && readErrors.Any()) {
                throw new ArgumentException($"Invalid configuration:\r\n {string.Join("\r\n", readErrors)}");
            }

            AddAlterationRules(ruleSet!.Rules.SelectMany(s => s.GetAllRules()));

            return this;
        }

        /// <summary>
        ///     If true, eryfit will act directly as the web server.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public EryfitSetting SetReverseMode(bool value)
        {
            ReverseMode = value;

            return this;
        }

        /// <summary>
        ///     Set Reverse mode forced port
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public EryfitSetting SetReverseModeForcedPort(int port)
        {
            ReverseModeForcedPort = port;

            return this;
        }

        /// <summary>
        ///     When true, eryfit will expect plain HTTP directly from the client.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public EryfitSetting SetReverseModePlainHttp(bool value)
        {
            if (value) {
                ReverseMode = true;
            }

            ReverseModePlainHttp = value;

            return this;
        }

        /// <summary>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public EryfitSetting SetVerbose(bool value)
        {
            Verbose = value;

            return this;
        }

        /// <summary>
        ///     Use the managed BouncyCastle engine
        /// </summary>
        /// <returns></returns>
        public EryfitSetting UseBouncyCastleSslEngine()
        {
            UseBouncyCastle = true;

            return this;
        }

        /// <summary>
        ///     Use the default SSL Engine provided by the operating system
        /// </summary>
        /// <returns></returns>
        public EryfitSetting UseOsSslEngine()
        {
            UseBouncyCastle = false;

            return this;
        }

        /// <summary>
        ///     Set the directory of the certificate cache.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public EryfitSetting SetCertificateCacheDirectory(string path)
        {
            CertificateCacheDirectory = path;

            return this;
        }

        /// <summary>
        ///     Set up a new rule adding chain
        /// </summary>
        /// <returns></returns>
        public IConfigureFilterBuilder ConfigureRule()
        {
            var addFilter = new ConfigureFilterBuilderBuilder(this);

            return addFilter;
        }

        /// <summary>
        /// </summary>
        /// <param name="proxyAuthentication"></param>
        /// <returns></returns>
        public EryfitSetting SetProxyAuthentication(ProxyAuthentication proxyAuthentication)
        {
            ProxyAuthentication = proxyAuthentication;

            return this;
        }

        /// <summary>
        /// </summary>
        /// <param name="configurationFile"></param>
        /// <returns></returns>
        public EryfitSetting SetUserAgentActionConfigurationFile(string configurationFile)
        {
            UserAgentActionConfigurationFile = configurationFile;

            return this;
        }
    }
}

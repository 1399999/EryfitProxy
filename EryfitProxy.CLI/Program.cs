namespace EryfitProxy.CLI;

public class Program
{
    static void Main(string[] args)
    {
        EryfitSetting eryfitSetting = Settings.Navigate();

        Start: { }

        Console.Clear();
        Console.WriteLine(" ==================== Eryfit 0.0.0 ====================\n");

        while (true)
        {
            Console.Write("> ");

            string[] inputVectors = Console.ReadLine().Trim().Split(' ');

            if ((inputVectors[0] == "setting" || inputVectors[0] == "settings") && Arguments.CheckForNoArguments(inputVectors))
            {
                eryfitSetting = Settings.Navigate();
                goto Start;
            }

            else if ((inputVectors[0] == "setting" || inputVectors[0] == "settings") && Arguments.CheckForHelp(inputVectors))
            {
                Information.DisplaySettingsHelp();
            }

            else if (inputVectors[0] == "help")
            {
                Information.DisplayGeneralHelp();
            }

            else if (inputVectors[0] == "credit" || inputVectors[0] == "credits")
            {
                Information.DisplayCredits();
            }

            else if ((inputVectors[0] == "run" || inputVectors[0] == "start") && Arguments.CheckForNoArguments(inputVectors))
            {
                Information.DisplayStartData(eryfitSetting);
                CoreFunctionality.RunProxy(eryfitSetting);
            }

            else if ((inputVectors[0] == "run" || inputVectors[0] == "start") && Arguments.CheckForHelp(inputVectors))
            {
                Information.DisplayStartHelp();
            }

            else if (inputVectors[0] != string.Empty)
            {
                SystemError.DisplayGeneralCommandError(inputVectors[0]);
            }
        }
    }

    #region Sample 1

    static async Task Sample1()
    {
        var tempDirectory = "capture_dump";

        // Create a default run settings 
        var eryfitStartupSetting = EryfitSetting
                                    // listen on port 44344 on IPV4 loopback
                                    .CreateDefault(IPAddress.Loopback, 44344)
                                    // add optional extra binding address on IPV6 loopback
                                    .AddBoundAddress(IPAddress.IPv6Loopback, 44344)
                                    // set the temporary output directory
                                    .SetOutDirectory(tempDirectory)
                                    // instruct Eryfit to install the certificate to the default machine store
                                    .SetAutoInstallCertificate(true);

        // Create a proxy instance
        await using (var proxy = new Proxy(eryfitStartupSetting))
        {
            var endpoints = proxy.Run();

            using var httpClient = new HttpClient(new HttpClientHandler()
            {
                // We instruct the HttpClient to use the proxy
                Proxy = new WebProxy($"http://127.0.0.1:{endpoints.First().Port}"),
                UseProxy = true
            });

            // Make a request to a remote website
            using var response = await httpClient.GetAsync("https://www.eryfit.io/hello");

            // Eryfit is in full streaming mode, this means that the actual body content 
            // is only captured when the client reads it. 

            await (await response.Content.ReadAsStreamAsync()).CopyToAsync(Stream.Null);
        }

        // Packing the output files must be after the proxy dispose because some files may 
        // remain write-locked. 

        // Pack the files into fxzy file. This is the recommended file format as it can holds raw capture datas. 
        Packager.Export(tempDirectory, "mycapture.fxzy");

        // Pack the files into a HAR file
        Packager.ExportAsHttpArchive(tempDirectory, "mycapture.har");
    }

    #endregion
    #region Sample 2

    static async Task Sample2()
    {
        var tempDirectory = "filtered_dump";

        // Create a default run settings 
        var eryfitStartupSetting = EryfitSetting
                                   .CreateDefault(IPAddress.Loopback, 44344)
                                   .SetOutDirectory(tempDirectory);

        // Only filter with OnAuthorityReceived scope will be accepted
        eryfitStartupSetting.SetSaveFilter(new HostFilter("eryfit.io", StringSelectorOperation.EndsWith));
        eryfitStartupSetting.SetSaveFilter(new HostFilter("eryfit.io"));

        // We can combine multiple condition with a filter collection 
        eryfitStartupSetting.SetSaveFilter(new FilterCollection(
            new HostFilter("eryfit.io", StringSelectorOperation.EndsWith),
            new IsSecureFilter()
        )
        {
            Operation = SelectorCollectionOperation.And
        });

        // Create a proxy instance
        await using (var proxy = new Proxy(eryfitStartupSetting))
        {
            var endpoints = proxy.Run();

            using var httpClient = new HttpClient(new HttpClientHandler()
            {
                // We instruct the HttpClient to use the proxy
                Proxy = new WebProxy($"http://127.0.0.1:{endpoints.First().Port}"),
                UseProxy = true
            });

            await QueryAndForget(httpClient, HttpMethod.Get, "https://www.eryfit.io/");
            await QueryAndForget(httpClient, HttpMethod.Get, "https://www.google.com/");
        }

        // Packing the output files must be after the proxy dispose because some files may 
        // remain write-locked. 

        // Pack the files into fxzy file. This is the recommended file format as it can holds raw capture datas. 
        Packager.Export(tempDirectory, "filtered_dump.fxzy");
    }

    static async Task QueryAndForget(HttpClient client, HttpMethod method, string url)
    {
        // Make a request to a remote website
        using var response = await client.SendAsync(new HttpRequestMessage(method, url));

        var contentStream = await response.Content.ReadAsStreamAsync();

        // Eryfit is in full streaming mode, this means that the actual body content 
        // is only captured when the client reads it.
        // Here we drain the response stream to an Stream.Null
        await contentStream.CopyToAsync(Stream.Null);
    }

    static async Task Do()
    {
        var eryfitStartupSetting = EryfitSetting
                                   .CreateDefault(IPAddress.Loopback, 44344)
                                   .AddAlterationRules(
                                       new Rule(
                                           new AddResponseHeaderAction("X-Proxy", "Passed through eryfit"),
                                           AnyFilter.Default
                                       ));

        await using (var proxy = new Proxy(eryfitStartupSetting))
        {
            var _ = proxy.Run();

            Console.WriteLine("Press any key to exit...");
            Console.ReadLine();
        }
    }

    #endregion
    #region Sample 3

    static async void Sample3()
    {
        var tempDirectory = "raw_capture_dump";
        var extractNssKey = true;  // Change this value in order to enable/disable NSS key log file capture.

        // Create a default run settings 
        var eryfitStartupSetting = EryfitSetting
                                   // listen on port 44344 on IPV4 loopback
                                   .CreateDefault(IPAddress.Loopback, 44344)
                                   // add optional extra binding address on IPV6 loopback
                                   .AddBoundAddress(IPAddress.IPv6Loopback, 44344)
                                   // set the temporary output directory
                                   .SetOutDirectory(tempDirectory);

        if (extractNssKey)
        {
            // To enable nss key capture, the SSL engine used by Eryfit must be BouncyCastle 
            eryfitStartupSetting.UseBouncyCastleSslEngine();
        }

        await using (var tcpConnectionProvider = await CapturedTcpConnectionProvider.CreateInProcessCapture())
        {
            await using var proxy = new Proxy(eryfitStartupSetting, tcpConnectionProvider: tcpConnectionProvider);

            var endpoints = proxy.Run();

            using var httpClient = new HttpClient(new HttpClientHandler()
            {
                // We instruct the HttpClient to use the proxy
                Proxy = new WebProxy($"http://127.0.0.1:{endpoints.First().Port}"),
                UseProxy = true
            });

            // Make a request to a remote website
            using var response = await httpClient.GetAsync("https://www.example.com/");

            // Eryfit is in full streaming mode, this means that the actual body content 
            // is only captured when the client reads it. 
            await (await response.Content.ReadAsStreamAsync()).CopyToAsync(Stream.Null);
        }

        // Pack the files into fxzy file. This is the recommended file format as it can holds raw capture datas. 
        Packager.Export(tempDirectory, "mycapture.fxzy");

        // Exporting pcapng file 

        var archiveReader = new DirectoryArchiveReader(tempDirectory);

        var exchange = archiveReader.ReadAllExchanges().First(e => e.FullUrl == "https://www.example.com/");

        var rawCaptureStream = archiveReader.GetRawCaptureStream(exchange.ConnectionId);

        if (extractNssKey)
        {
            // Extract SSL key log file 
            var sslKeyLogContent = archiveReader.GetRawCaptureKeyStream(exchange.ConnectionId)!.ReadToEndGreedy();

            // Eryfit provides an utility to combine a pcapng file with a SSLKeyLogFile 

            await using var pcanPngFile = File.Create("out.with-keys.pcapng");
            await PcapngUtils.CreatePcapngFileWithKeysAsync(sslKeyLogContent, rawCaptureStream!, pcanPngFile);
        }
        else
        {
            await using var fileStream = File.Create("out.pcapng");
            await rawCaptureStream!.CopyToAsync(fileStream);
        }
    }

    #endregion
    #region Sample 4

    static async void Sample4()
    {
        var tempDirectory = "basic_alteration";

        // Create a default run settings 
        var eryfitSetting = EryfitSetting
                                   .CreateDefault(IPAddress.Loopback, 44344)
                                   .SetOutDirectory(tempDirectory);

        // The full list of available actions and rules are available at 
        // https://www.eryfit.io/rule/search


        eryfitSetting
            // Set up rule configuration in a fluent way
            .ConfigureRule()

            // Append "eryfit-on" header to any request 
            .WhenAny().Do(new AddRequestHeaderAction("eryfit-on", "true"))
            // Remove any cache directive from any request 
            .WhenAny().Do(new RemoveCacheAction())

            // Avoid decrypting particular host 
            .When(new HostFilter("secure.domain.com", StringSelectorOperation.Exact))
                .Do(new SkipSslTunnelingAction())

            // Mock an entire response according to an URL 
            .WhenUriMatch(@"^https\:\/\/api\.example\.com", StringSelectorOperation.Regex)
                .Do(new MockedResponseAction(MockedResponseContent.CreateFromPlainText("This is a plain text content")))

            // Using a client certificate
            .When(new HostFilter("domain.with.mandatory.client.cert.com", StringSelectorOperation.Exact))
                .Do(new SetClientCertificateAction(Certificate.LoadFromUserStoreBySerialNumber("xxxxxx")));


        // Create a proxy instance
        await using (var proxy = new Proxy(eryfitSetting))
        {
            var endpoints = proxy.Run();

            using var httpClient = new HttpClient(new HttpClientHandler()
            {
                // We instruct the HttpClient to use the proxy
                Proxy = new WebProxy($"http://127.0.0.1:{endpoints.First().Port}"),
                UseProxy = true
            });

            await QueryAndForget(httpClient, HttpMethod.Get, "https://www.eryfit.io/");
            await QueryAndForget(httpClient, HttpMethod.Get, "https://www.google.com/");
            await QueryAndForget(httpClient, HttpMethod.Get, "https://api.example.com/random_endpoint");
        }

        // Packing the output files must be after the proxy dispose because some files may 
        // remain write-locked. 

        // Pack the files into fxzy file. This is the recommended file format as it can holds raw capture datas. 
        Packager.Export(tempDirectory, "altered.fxzy");
    }

    #endregion
    #region Sample 5

    static async void Sample5()
    {
        var tempDirectory = "basic_alteration_with_configuration_file";

        // Create a default run settings 
        var eryfitStartupSetting = EryfitSetting
                                   .CreateDefault(IPAddress.Loopback, 44344)
                                   .SetOutDirectory(tempDirectory);

        // The full list of available actions and rules are available at 
        // https://www.eryfit.io/rule/search

        var yamlContent = """
                rules:
                  - filter: 
                      typeKind: AnyFilter        
                    action : 
                      typeKind: AddRequestHeaderAction
                      headerName: eryfit
                      headerValue: on
                  - filter: 
                      typeKind: StatusCodeRedirectionFilter        
                    action : 
                      typeKind: ApplyCommentAction
                      comment: This is a redirection
                """;

        // Creating ruleset from raw yaml content   
        eryfitStartupSetting.AddAlterationRules(yamlContent);

        // Create a proxy instance
        await using (var proxy = new Proxy(eryfitStartupSetting))
        {
            var endpoints = proxy.Run();

            using var httpClient = new HttpClient(new HttpClientHandler()
            {
                // We instruct the HttpClient to use the proxy
                Proxy = new WebProxy($"http://127.0.0.1:{endpoints.First().Port}"),
                UseProxy = true
            });

            await QueryAndForget(httpClient, HttpMethod.Get, "https://www.eryfit.io/");
        }

        // Packing the output files must be after the proxy dispose because some files may 
        // remain write-locked. 

        // Pack the files into fxzy file. This is the recommended file format as it can holds raw capture datas. 
        Packager.Export(tempDirectory, "altered.fxzy");
    }

    #endregion
    #region Sample 6

    static async void Sample6()
    {
        // Create a default run settings 
        var eryfitStartupSetting = EryfitSetting.CreateLocalRandomPort();

        // Create a proxy instance
        await using var proxy = new Proxy(eryfitStartupSetting);

        var endpoints = proxy.Run();

        await using var proxyRegistration = await SystemProxyRegistrationHelper.Create(endpoints.First());

        // Eryfit is now registered as the system proxy, the proxy will revert
        // back to the original settings when proxyRegistration is disposed.

        Console.WriteLine("Press any key to halt proxy and unregistered");

        Console.ReadKey();
    }

    #endregion
    #region Sample 7

    static async void Sample7()
    {
        var eryfitSetting = EryfitSetting.CreateDefault();

        // Load from a PKCS12 file (pfx and p12)
        var certificate = Certificate.LoadFromPkcs12("clientCertificiate.p12", "password");

        // Load from user store with serial number
        // var certificate = Certificate.LoadFromUserStoreBySerialNumber("xxxx")

        // Load from user store with thumbprint
        // var certificate = Certificate.LoadFromUserStoreByThumbprint("xxxxx");

        eryfitSetting.ConfigureRule()
                     .WhenHostEndsWith("mtls-mandatory.com")
                     .Do(new SetClientCertificateAction(certificate));

        // Create a new proxy instance 
        await using var proxy = new Proxy(eryfitSetting);

        proxy.Run();

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
    }

    #endregion
    #region Sample 8

    static async void Sample8()
    {
        var eryfitSetting = EryfitSetting.CreateDefault();

        eryfitSetting.ConfigureRule()
                     .WhenHostMatch("must-be-http-11.com")
                     .Do(new ForceHttp11Action())
                     .WhenHostMatch("must-be-http-2.com")
                     .Do(new ForceHttp11Action());

        // Create a new proxy instance 
        await using var proxy = new Proxy(eryfitSetting);
        proxy.Run();

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
    }

    #endregion
    #region Sample 9

    static async void Sample9()
    {
        var eryfitSetting = EryfitSetting.CreateDefault();

        eryfitSetting.ConfigureRule()
                     // When the content type is text/html
                     .When(new RequestHeaderFilter("text/html", "content-type"))

                     // Inject the following html tag into the head tag
                     .Do(new InjectHtmlTagAction()
                     {
                         HtmlContent = "<style>body { background-color: red !important; }</style>",
                         Tag = "head", // we insert on tag header to execute the snippet early
                     });

        await using var proxy = new Proxy(eryfitSetting);

        proxy.Run();

        Console.WriteLine("Press any key to exit");

        Console.ReadKey();
    }

    #endregion
    #region Sample 10

    static async void Sample10()
    {
        var eryfitSetting = EryfitSetting.CreateDefault();

        eryfitSetting.ConfigureRule()
                     // Mock with an immediate response
                     .WhenHostMatch("www.google.com", StringSelectorOperation.StartsWith)
                     .Do(new MockedResponseAction(
                         MockedResponseContent.CreateFromPlainText("This is a plain text content")));

        await using var proxy = new Proxy(eryfitSetting);

        proxy.Run();

        Console.WriteLine("Press any key to exit");

        Console.ReadKey();
    }

    #endregion
    #region Sample 11

    static async void Sample11()
    {
        var eryfitSetting = EryfitSetting.CreateDefault();

        eryfitSetting.ConfigureRule()
                     // Add request cookie on any ongoing request
                     .WhenAny()
                     .Do(new SetRequestCookieAction("eryfit-cookie", "yes"));

        await using var proxy = new Proxy(eryfitSetting);

        proxy.Run();

        Console.WriteLine("Press any key to exit");

        Console.ReadKey();
    }

    #endregion
    #region Sample 12

    static async void Sample12()
    {
        var eryfitSetting = EryfitSetting.CreateDefault();

        eryfitSetting.ConfigureRule()
                     // Add response cookie on any ongoing response
                     .WhenAny()
                     .Do(new SetResponseCookieAction("eryfit-response-cookie", "sweet")
                     {
                         Path = "/",
                         ExpireInSeconds = 3600,
                         HttpOnly = true,
                         Secure = true,
                         SameSite = "Lax",
                         MaxAge = 3600,
                         // Domain =  -- set domain here 
                     });

        await using var proxy = new Proxy(eryfitSetting);

        proxy.Run();

        Console.WriteLine("Press any key to exit");

        Console.ReadKey();
    }

    #endregion
    #region Sample 13

    static async void Sample13()
    {
        var eryfitSetting = EryfitSetting.CreateDefault();

        eryfitSetting.ConfigureRule()

                     // Add response cookie on any ongoing response
                     .WhenAny()
                     .Do(
                         new AddRequestHeaderAction("new-header", "value"),
                         new DeleteRequestHeaderAction("Date"), // Delete request header
                         new UpdateRequestHeaderAction("User-Agent", "{{previous}} - add suffix to user-agent"),
                         new AddResponseHeaderAction("new-response-header", "value"),
                         new DeleteResponseHeaderAction("Date"), // Delete response header
                         new UpdateResponseHeaderAction("Server", "{{previous}} - add suffix to server"));

        await using var proxy = new Proxy(eryfitSetting);

        proxy.Run();

        Console.WriteLine("Press any key to exit");

        Console.ReadKey();
    }

    #endregion
    #region Sample 15

    static async void Sample15()
    {
        // raw captured file
        var tempFile = $"output.raw.pcapng";

        // captured file combined with keys
        var decodedFile = $"output.decoded.pcapng";

        {
            // Handler must be disposed to access the captured data

            using var handler = await PcapngUtils.CreateHttpHandler(tempFile);
            using var httpClient = new HttpClient(handler);
            using var _ = await httpClient.GetAsync("https://www.example.com");
        }

        await using var outStream = File.Create(decodedFile);

        // Utility to read the pcapng file with the included keys if available
        await PcapngUtils.ReadWithKeysAsync(tempFile).CopyToAsync(outStream);

        // Fail if you don't have Wireshark installed or a compatible pcapng reader
        Process.Start(new ProcessStartInfo(decodedFile)
        {
            UseShellExecute = true
        });
    }

    #endregion
    #region Sample 16

    static async void Sample16()
    {
        var archiveReader = new EryfitArchiveReader("example.com.fxzy");

        // Use DirectoryArchiveReader to read from a dump directory 
        // var archiveReader = new DirectoryArchiveReader("directory_name")

        var allExchanges = archiveReader.ReadAllExchanges();

        var exchange = allExchanges.First();

        foreach (var header in exchange.GetRequestHeaders())
        {
            Console.WriteLine($"{header.Name}: {header.Value}");
        }

        Console.WriteLine();

        foreach (var header in exchange.GetResponseHeaders()!)
        {
            Console.WriteLine($"{header.Name}: {header.Value}");
        }

        var responseBodyStream = archiveReader.GetDecodedResponseBody(exchange.Id)!;

        var responseAsString = responseBodyStream.ReadToEndGreedy();

        Console.WriteLine(responseAsString);
    }

    #endregion
    #region Sample 17

    static async void Sample17()
    {
        // Create a default run settings 
        var eryfitStartupSetting = EryfitSetting.CreateLocalRandomPort();

        // Mandatory, BouncyCastle must be used to reproduce the fingerprints
        eryfitStartupSetting.UseBouncyCastleSslEngine();

        // Add an impersonation rule for Chrome 131
        eryfitStartupSetting.AddAlterationRulesForAny(
            new ImpersonateAction(ImpersonateProfileManager.Chrome131Windows));

        // Create a proxy instance
        await using var proxy = new Proxy(eryfitStartupSetting);

        var endpoints = proxy.Run();

        await using var proxyRegistration = await SystemProxyRegistrationHelper.Create(endpoints.First());

        // Eryfit is now registered as the system proxy, the proxy will revert
        // back to the original settings when proxyRegistration is disposed.

        Console.WriteLine("Press any key to halt proxy and unregistered");

        Console.ReadKey();
    }

    #endregion
}

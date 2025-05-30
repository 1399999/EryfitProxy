namespace EryfitProxy.Kernel.Cli
{
    public static class EryfitStartup
    {
        public static async Task<int> Run(string[] args, OutputConsole? outputConsole, CancellationToken token, 
            EnvironmentProvider? environmentProvider = null)
        {
            var currentEnvironmentProvider = environmentProvider ?? new SystemEnvironmentProvider();

            if (currentEnvironmentProvider.GetEnvironmentVariable("ERYFIT_STDOUT_ARGS") == "1") {

                if (outputConsole == null)
                {
                    Console.WriteLine(string.Join(" ", args));
                }
                else
                {
                    outputConsole.WriteLine(string.Join(" ", args));
                }
            }

            var rootCommand =
                new RootCommand(
                    "CLI tool for recording, analyzing and altering HTTP/1.1, H2, WebSocket traffic over plain or secure channels. Visit https://eryfit.io for more info.");

            var instanceIdentifier = Guid.NewGuid().ToString();

            var startCommandBuilder = new StartCommandBuilder(instanceIdentifier);
            var certificateCommandBuilder = new CertificateCommandBuilder();
            var packCommandBuilder = new PackCommandBuilder();
            var dissectCommandBuilder = new DissectCommandBuilder();

            rootCommand.Add(startCommandBuilder.Build(token));
            rootCommand.Add(certificateCommandBuilder.Build(currentEnvironmentProvider));
            rootCommand.Add(packCommandBuilder.Build());
            rootCommand.Add(dissectCommandBuilder.Build());

            var final = new CommandLineBuilder(rootCommand)
                        .UseVersionOption("-v", "--version")
                        .UseHelp()
                        .UseEnvironmentVariableDirective()
                        .UseParseDirective()
                        .UseTypoCorrections()
                        .UseParseErrorReporting()
                        .CancelOnProcessTermination()
                        .UseExceptionHandler((e, context) => {
                            Console.ForegroundColor = ConsoleColor.Red;
                            context.Console.Error.WriteLine(e.Message);
                            Console.ResetColor();
                            context.ExitCode = 1;
                        }, 1)
                        .Build();

            try {
                var exitCode = outputConsole == null
                    ? await final.InvokeAsync(args)
                    : await final.InvokeAsync(args, outputConsole);

                return exitCode;
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);

                return 1;
            }
        }
    }
}

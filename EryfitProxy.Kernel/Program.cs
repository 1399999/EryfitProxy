namespace EryfitProxy.Kernel.Cli
{
    internal static class Program
    {
        internal static async Task<int> Main(string[] args)
        {
            var environmentProvider = new SystemEnvironmentProvider();

            if (Environment.GetEnvironmentVariable("appdata") == null) {
                Environment.SetEnvironmentVariable("appdata", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
            }

            var silentConsole = environmentProvider.EnvironmentVariableActive("ERYFIT_NO_STDOUT");
            
            var outputConsole = silentConsole ? OutputConsole.CreateEmpty() : null ;

            var exitCode = await EryfitStartup.Run(args, outputConsole, CancellationToken.None);

            return exitCode;
        }
    }
}

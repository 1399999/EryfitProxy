namespace EryfitProxy.Kernel.Cli.Commands
{
    public static class ConsoleHelper
    {
        public static void WriteValidationResult(this IConsole console, ValidationResult validationResult)
        {
            var consoleColor = Console.ForegroundColor;

            if (console is SystemConsole) {
                if (validationResult.Level == ValidationRuleLevel.Information) {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                }
                else if (validationResult.Level == ValidationRuleLevel.Warning) {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                }
                else {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
            }

            try {
                console.WriteLine(
                    $@"[{validationResult.Level} {validationResult.SenderName}] {validationResult.Message}");
            }
            finally {
                if (console is SystemConsole) {
                    Console.ForegroundColor = consoleColor;
                }
            }
        }
    }
}

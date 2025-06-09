namespace EryfitProxy.CLI;

public class SystemError
{
    public static void DisplayGeneralCommandError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;

        Console.WriteLine("ERROR\n");
        Console.WriteLine($"> {message}");
        Console.WriteLine($"The command is invalid. Use the \"help\" command for help.");

        Console.ForegroundColor = ConsoleColor.White;
    }
}

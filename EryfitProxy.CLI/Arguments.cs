namespace EryfitProxy.CLI;

public class Arguments
{
    public static bool CheckForNoArguments(string[] commandVectors) => commandVectors.Length == 1;
    public static bool CheckForHelp(string[] commandVectors) => commandVectors.Length <= 1 ? false : commandVectors[1] == "help" || commandVectors[1] == "--help";
}

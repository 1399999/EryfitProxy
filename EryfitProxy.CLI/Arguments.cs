namespace EryfitProxy.CLI;

public class Arguments
{
    public static bool CheckForNoArguments(string[] fullCommand) => fullCommand.Length == 1;
}

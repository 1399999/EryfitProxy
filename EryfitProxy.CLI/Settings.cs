namespace EryfitProxy.CLI;

public class Settings
{
    public static void Navigate()
    {
        SystemConsole.ConvertIntoOptionsMode(OptionsSettings, OptionsInputMap, OptionsBottomMessages);

        //if (index == 0)
        //{
        //    // 
        //}
    }

    public static List<string> OptionsSettings = new()
    {
        "IP Address Option",
        "Port",
        "New Bound Address Option",
        "New Bound Address Port",
        "Output Directory",
        "Install Certificate"
    };

    public static List<(bool, string?)> OptionsInputMap = new()
    {
        (true,"Loopback"),
        (true,"44344"),
        (true,null),
        (true,null),
        (true,null),
        (true,"false"),
    };

    public static List<string?> OptionsBottomMessages = new()
    {
        "Options: Any, Broadcast, IPv6Any, IPv6Loopback, IPv6None, Loopback, None",
        "<Port Number>",
        "<Optional>",
        "<Optional>",
        "Full or partial path for the output directory.",
        "Options: true or false.",
    };
}

namespace EryfitProxy.CLI;

public class Settings
{
    public delegate bool ValidationMethods(string str);

    public static EryfitSetting Navigate()
    {
        EryfitSetting setting = new EryfitSetting();

        List<string?> newOptions = SystemConsole.ConvertIntoOptionsMode(OptionsSettings, OptionsInputMap, OptionsBottomMessages, ValidationOptionsMap);
        setting = EryfitSetting.CreateDefault(ToIPAddressOption(newOptions[0]), int.Parse(newOptions[1]));

        if (newOptions[2] != null && newOptions[3] != null)
        {
            setting.AddBoundAddress(ToIPAddressOption(newOptions[2]), int.Parse(newOptions[3]));
        }

        if (newOptions[4] != null)
        {
            setting.SetOutDirectory(newOptions[4]);
        }

        if (newOptions[5] != null)
        {
            setting.SetAutoInstallCertificate(bool.Parse(newOptions[5]));
        }

        return setting;
    }

    public static IPAddress ToIPAddressOption(string? option)
    {
        if (option == "Any")
        {
            return IPAddress.Any;
        }

        else if (option == "Broadcast")
        {
            return IPAddress.Broadcast;
        }

        else if (option == "IPv6Any")
        {
            return IPAddress.IPv6Any;
        }

        else if (option == "IPv6Loopback")
        {
            return IPAddress.IPv6Loopback;
        }

        else if (option == "IPv6None")
        {
            return IPAddress.IPv6None;
        }

        else if (option == "None")
        {
            return IPAddress.None;
        }

        else 
        {
            return IPAddress.Loopback;
        }
    }

    private static List<string> OptionsSettings = new()
    {
        "IP Address Option",
        "Port",
        "New Bound Address Option",
        "New Bound Address Port",
        "Output Directory",
        "Install Certificate"
    };

    private static List<(bool, string?)> OptionsInputMap = new()
    {
        (true,"Loopback"),
        (true,"44344"),
        (true,null),
        (true,null),
        (true,null),
        (true,"false"),
    };

    private static List<string?> OptionsBottomMessages = new()
    {
        "Options: Any, Broadcast, IPv6Any, IPv6Loopback, IPv6None, Loopback, None",
        "<Port Number>",
        "<Optional>",
        "<Optional>",
        "Full or partial path for the output directory.",
        "Options: true or false.",
    };

    private static List<ValidationMethods> ValidationOptionsMap = new()
    {
        Validation.IsIPAddressOptions,
        Validation.IsInt, 
        Validation.IsIPAddressOptions,
        Validation.IsInt,
        Validation.IsDirectory,
        Validation.IsBool,
    };
}

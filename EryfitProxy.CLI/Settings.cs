<<<<<<< HEAD
using EryfitProxy.Kernel.Rules.Filters;

=======
>>>>>>> acce00391cb61667112a49109ffb1deaa27cc84b
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

<<<<<<< HEAD
        if (newOptions[6] != null)
        {
            setting.SetSaveFilter(new HostFilter(newOptions[6]));
        }

        if (newOptions[7] != null)
        {
            setting.SetSaveFilter(new HostFilter(newOptions[7], (StringSelectorOperation) ToStringSelectorOperation(newOptions[7])));
        }

        return setting;
    }

=======
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

>>>>>>> acce00391cb61667112a49109ffb1deaa27cc84b
    private static List<string> OptionsSettings = new()
    {
        "IP Address Option",
        "Port",
        "New Bound Address Option",
        "New Bound Address Port",
        "Output Directory",
<<<<<<< HEAD
        "Install Certificate",
        "New Host Filter Pattern",
        "New Host Filter String Selector",
=======
        "Install Certificate"
>>>>>>> acce00391cb61667112a49109ffb1deaa27cc84b
    };

    private static List<(bool, string?)> OptionsInputMap = new()
    {
        (true,"Loopback"),
        (true,"44344"),
        (true,null),
        (true,null),
        (true,null),
        (true,"false"),
<<<<<<< HEAD
        (true,null),
        (true,null),
=======
>>>>>>> acce00391cb61667112a49109ffb1deaa27cc84b
    };

    private static List<string?> OptionsBottomMessages = new()
    {
        "Options: Any, Broadcast, IPv6Any, IPv6Loopback, IPv6None, Loopback, None",
        "<Port Number>",
        "<Optional>",
        "<Optional>",
        "Full or partial path for the output directory.",
        "Options: true or false.",
<<<<<<< HEAD
        "Options: <URL>.",
        "Options: Contains, EndsWith, Exact, Regex, StartsWith.",
=======
>>>>>>> acce00391cb61667112a49109ffb1deaa27cc84b
    };

    private static List<ValidationMethods> ValidationOptionsMap = new()
    {
        Validation.IsIPAddressOptions,
        Validation.IsInt, 
        Validation.IsIPAddressOptions,
        Validation.IsInt,
        Validation.IsDirectory,
        Validation.IsBool,
<<<<<<< HEAD
        Validation.None,
        Validation.IsStringSelectorOperation,
    };

    public static IPAddress ToIPAddressOption(string? option)
    {
        if (option.ToLower() == "any")
        {
            return IPAddress.Any;
        }

        else if (option.ToLower() == "broadcast")
        {
            return IPAddress.Broadcast;
        }

        else if (option.ToLower() == "ipv6any")
        {
            return IPAddress.IPv6Any;
        }

        else if (option.ToLower() == "ipv6loopback")
        {
            return IPAddress.IPv6Loopback;
        }

        else if (option.ToLower() == "ipv6none")
        {
            return IPAddress.IPv6None;
        }

        else if (option.ToLower() == "none")
        {
            return IPAddress.None;
        }

        else
        {
            return IPAddress.Loopback;
        }
    }

    public static StringSelectorOperation? ToStringSelectorOperation(string? option)
    {
        if (option.ToLower() == "contains")
        {
            return StringSelectorOperation.Contains;
        }

        else if (option.ToLower() == "endswith")
        {
            return StringSelectorOperation.EndsWith;
        }

        else if (option.ToLower() == "exact")
        {
            return StringSelectorOperation.Exact;
        }

        else if (option.ToLower() == "regex")
        {
            return StringSelectorOperation.Regex;
        }

        else if (option.ToLower() == "startswith")
        {
            return StringSelectorOperation.StartsWith;
        }

        else
        {
            return null;
        }
    }
=======
    };
>>>>>>> acce00391cb61667112a49109ffb1deaa27cc84b
}

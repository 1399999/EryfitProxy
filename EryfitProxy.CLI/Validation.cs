namespace EryfitProxy.CLI;

public static class Validation
{
    public static bool IsInt(string str) => int.TryParse(str, out _);
    public static bool IsIPAddressOptions(string str) => str == "Any" || str == "Broadcast" || str == "IPv6Any" || str == "IPv6Loopback" || str == "IPv6None" || str == "Loopback" || str == "None";

    public static bool IsDirectory(string str) => Directory.Exists(str);
    public static bool IsBool(string str) => bool.TryParse(str, out _);
}

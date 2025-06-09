namespace EryfitProxy.CLI;

public static class Validation
{
    public static bool IsInt(string str) => int.TryParse(str, out _);
    public static bool IsIPAddressOptions(string str) => str.ToLower() == "any" || str.ToLower() == "broadcast" || str.ToLower() == "ipv6any" || str.ToLower() == "ipv6loopback" || str.ToLower() == "ipv6none" || str.ToLower() == "loopback" || str.ToLower() == "none";
    public static bool IsStringSelectorOperation(string str) => str.ToLower() == "contains" || str.ToLower() == "endswith" || str.ToLower() == "exact" || str.ToLower() == "regex" || str.ToLower() == "startswith";
    public static bool IsDirectory(string str) => Directory.Exists(str);
    public static bool IsBool(string str) => bool.TryParse(str, out _);
    public static bool None(string str) => true;
}

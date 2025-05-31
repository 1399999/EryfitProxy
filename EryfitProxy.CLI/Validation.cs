namespace EryfitProxy.CLI;

public static class Validation
{
    public static bool IsInt(this string str) => int.TryParse(str, out _);
}

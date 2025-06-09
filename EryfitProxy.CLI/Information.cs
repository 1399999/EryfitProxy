namespace EryfitProxy.CLI;

public class Information
{
    public static void DisplayGeneralHelp() => Console.WriteLine
    (
        "\nCommands:\n\n" +
        "credits: Displays the credits screen.\n" +
        "settings [--help]: Opens the settings options menu.\n" +
        "help: Displays this menu.\n"
    );

    public static void DisplaySettingsHelp() => Console.WriteLine
    (
        "\nDisplays the options menu for setting up an instance of Eryfit Proxy.\n"
    );

    public static void DisplayCredits() => Console.WriteLine
    (
        "\nCopyright (C) Mikhail Zhebrunov 2020-2025.\n"
    );
}

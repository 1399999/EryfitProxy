namespace EryfitProxy.CLI;

public class SystemConsole
{

    /// <summary>
    /// Turns the console into a selection of options (more simple).
    /// </summary>
    /// <param name="options">The parmaters which the users can choose from (in list form).</param>
    /// <returns>The index which the user selects.</returns>
    public static int ConvertIntoOptionsMode(List<string> options)
    {
        int currentIndex = 0;

        Console.CursorVisible = false;

        while (true)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;

            Console.Clear();

            for (int i = 0; i < options.Count; i++)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Black;

                if (currentIndex == i)
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.White;
                }

                Console.WriteLine(options[i]);
            }

            var key = Console.ReadKey().Key;

            if (key == ConsoleKey.DownArrow)
            {
                currentIndex = currentIndex < options.Count - 1 ? currentIndex + 1 : 0;
                continue;
            }

            else if (key == ConsoleKey.UpArrow)
            {
                currentIndex = currentIndex - 1 >= 0 ? currentIndex - 1 : options.Count - 1;
                continue;
            }

            else if (key == ConsoleKey.Enter)
            {
                return currentIndex;
            }

            else
            {
                continue;
            }
        }
    }

    /// <summary>
    /// Turns the console into a selection of options (more complex).
    /// </summary>
    /// <param name="options">The parmaters which the users can choose from.</param>
    /// <param name="OptionsInputMap">Whether the option has text to it, instead of just being an option. If yes, its default value.</param>
    /// <param name="OptionsBottomMessages">The bottom message that displays below the options.</param>
    /// <param name="validationOptionsMap">Validation functions that the program uses (delegates).</param>
    /// <returns>The outputs which the user selects.</returns>
    public static List<string?> ConvertIntoOptionsMode(List<string> options, List<(bool, string?)> OptionsInputMap, List<string?> OptionsBottomMessages, List<Settings.ValidationMethods> validationOptionsMap)
    {
        int currentIndex = 0;

        List<string?> output = new();

        Console.CursorVisible = false;

        for (int i = 0; i < options.Count; i++)
        {
            var b = OptionsInputMap[i];
            output.Add(b.Item2);
        }

        while (true)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;

            Console.Clear();

            for (int i = 0; i < options.Count; i++)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Black;

                if (currentIndex == i)
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.White;
                }

                Console.WriteLine(options[i]);
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;

            var a = OptionsInputMap[currentIndex];

            var haveInput = a.Item1;
            var defaultInput = output[currentIndex];

            if (haveInput)
            {
                Console.CursorVisible = true;

                Console.WriteLine($"\nPress <Enter> and then start typing the new value in.");
                Console.WriteLine(OptionsBottomMessages[currentIndex]);

                Console.WriteLine($"\nOld Value: {defaultInput}");
                Console.Write($"New Value: ");
            }

            var key = Console.ReadKey().Key;

            if (key == ConsoleKey.DownArrow)
            {
                currentIndex = currentIndex < options.Count - 1 ? currentIndex + 1 : 0;
                continue;
            }

            else if (key == ConsoleKey.UpArrow)
            {
                currentIndex = currentIndex - 1 >= 0 ? currentIndex - 1 : options.Count - 1;
                continue;
            }

            else if (key == ConsoleKey.Enter)
            {
                if (!haveInput)
                {
                    return output;
                }

                else
                {
                    var input = Console.ReadLine();

                    if (validationOptionsMap[currentIndex](input))
                    {
                        output[currentIndex] = input;
                    }

                    continue;
                }
            }

            else if (key == ConsoleKey.Escape)
            {
                return output;
            }

            else
            {
                continue;
            }
        }
    }
}

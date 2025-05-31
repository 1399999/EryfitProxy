namespace EryfitProxy.CLI;

public class SystemConsole
{
    /// <summary>
    /// Turns the console into a selection of options.
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
    /// Turns the console into a selection of options.
    /// </summary>
    /// <param name="options"></param>
    /// <param name="OptionsInputMap"></param>
    /// <param name="OptionsBottomMessages"></param>
    /// <returns></returns>
    public static List<string?> ConvertIntoOptionsMode(List<string> options, List<(bool, string?)> OptionsInputMap, List<string?> OptionsBottomMessages)
    {
        int currentIndex = 0;

        List<string?> Output = new();

        Console.CursorVisible = false;

        for (int i = 0; i < options.Count; i++)
        {
            var b = OptionsInputMap[i];
            Output.Add(b.Item2);
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
            var defaultInput = Output[currentIndex];

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
                    return Output;
                }

                else
                {
                    // TODO: Assign Variable.
                    Output[currentIndex] = Console.ReadLine();
                    continue;
                }
            }

            else if (key == ConsoleKey.Escape)
            {
                return Output;
            }

            else
            {
                continue;
            }
        }
    }
}

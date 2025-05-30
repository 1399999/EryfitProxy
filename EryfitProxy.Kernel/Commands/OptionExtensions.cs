namespace EryfitProxy.Kernel.Cli.Commands
{
    public static class OptionExtensions
    {
        public static Option<T> Get<T>(this IEnumerable<Option> options, string name)
        {
            return options.OfType<Option<T>>().First(t => t.Name == name);
        }

        public static T Value<T>(this InvocationContext context, string name)
        {
            var command = context.ParseResult.CommandResult.Command;

            var option = command.Options.Get<T>(name);

            return (T) context.ParseResult.GetValueForOption(option)!;
        }
    }
}

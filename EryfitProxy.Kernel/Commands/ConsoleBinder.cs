namespace EryfitProxy.Kernel.Cli.Commands
{
    public class ConsoleBinder : BinderBase<IConsole>
    {
        protected override IConsole GetBoundValue(BindingContext bindingContext)
        {
            return bindingContext.Console;
        }
    }
}

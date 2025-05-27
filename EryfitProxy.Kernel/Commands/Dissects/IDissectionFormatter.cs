namespace EryfitProxy.Kernel.Cli.Commands.Dissects
{
    internal interface IDissectionFormatter<in T>
    {
        string Indicator { get; }

        Task Write(T payload, StreamWriter stdOutWriter);
    }
}

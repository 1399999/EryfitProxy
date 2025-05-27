namespace EryfitProxy.Kernel.Cli.Commands.Dissects
{
    internal record EntryInfo(ExchangeInfo Exchange, ConnectionInfo? Connection, IArchiveReader ArchiveReader);
}

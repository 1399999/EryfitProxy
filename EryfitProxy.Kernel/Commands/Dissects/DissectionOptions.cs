namespace EryfitProxy.Kernel.Cli.Commands.Dissects
{
    internal class DissectionOptions
    {
        public DissectionOptions(
            bool mustBeUnique,
            HashSet<int>? exchangeIds, string format)
        {
            MustBeUnique = mustBeUnique;
            ExchangeIds = exchangeIds;
            Format = format;
        }

        public string Format { get; }

        public bool MustBeUnique { get; }

        public HashSet<int>? ExchangeIds { get; }
    }
}

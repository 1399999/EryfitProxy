

using System;
using EryfitProxy.Kernel.Core;
using EryfitProxy.Kernel.Writers;

namespace EryfitProxy.Kernel
{
    public class ExchangeUpdateEventArgs : EventArgs
    {
        public ExchangeUpdateEventArgs(
            ExchangeInfo exchangeInfo,
            Exchange original, ArchiveUpdateType updateType)
        {
            ExchangeInfo = exchangeInfo;
            Original = original;
            UpdateType = updateType;
        }

        public ExchangeInfo ExchangeInfo { get; }

        public Exchange Original { get; }

        public ArchiveUpdateType UpdateType { get; }
    }
}

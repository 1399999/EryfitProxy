using System;

namespace EryfitProxy.Kernel.Rules.Actions
{
    [Flags]
    public enum ThrottleChannel
    {
        None,
        Request = 1,
        Response = 2,
        All = Request | Response
    }
}
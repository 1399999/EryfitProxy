using System;

namespace EryfitProxy.Kernel.Core
{
    internal class SystemEnvironmentProvider : EnvironmentProvider
    {
        public override string? GetEnvironmentVariable(string variable)
        {
            return Environment.GetEnvironmentVariable(variable);
        }

        public override string ExpandEnvironmentVariables(string original)
        {
            return Environment.ExpandEnvironmentVariables(original);
        }
    }
}
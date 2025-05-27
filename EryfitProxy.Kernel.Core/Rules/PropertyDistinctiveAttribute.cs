

using System;

namespace EryfitProxy.Kernel.Rules
{
    public class PropertyDistinctiveAttribute : Attribute
    {
        public bool Expand { get; set; }

        public bool IgnoreInDoc { get; set; } = false;

        public string Description { get; set; } = string.Empty;

        public string? DefaultValue { get; set; }

        public string ? FriendlyType { get; set; }

    }
}



using System;

namespace EryfitProxy.Kernel.Rules
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ActionMetadataAttribute : Attribute
    {
        public ActionMetadataAttribute(string longDescription)
        {
            LongDescription = longDescription;
        }

        public string LongDescription { get; }

        public bool NonDesktopAction { get; set; }
    }
}

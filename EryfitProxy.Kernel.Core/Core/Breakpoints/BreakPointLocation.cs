

using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace EryfitProxy.Kernel.Core.Breakpoints
{
    public enum BreakPointLocation
    {
        Start = 0,
        
        [Description("Connection set up")]
        PreparingRequest = 1,

        [Description("Edit request")]
        Request,

        [Description("Edit response")]
        Response,
    }

    public static class DescriptionHelper
    {
        public static string? GetEnumDescription(this Enum value)
        {
            var type = value.GetType();
            var memberInfos = type.GetMember(value.ToString());
            var attribute = memberInfos.FirstOrDefault()?.GetCustomAttribute<DescriptionAttribute>(); 
            return attribute?.Description;
        }
    }
}

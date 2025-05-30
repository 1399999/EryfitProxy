

using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace EryfitProxy.Kernel.Rules.Filters
{
    public static class FilterScopeExtensions
    {
        public static string GetDescription(this FilterScope filterScope)
        {
            var type = filterScope.GetType();
            var memberInfo = type.GetMember(filterScope.ToString()).First();
            var attribute = memberInfo.GetCustomAttribute<DescriptionAttribute>();

            if (attribute == null)
                throw new InvalidOperationException("Provided enum does not have DescriptionAttribute"); 

            return attribute.Description; 

        }
    }
}

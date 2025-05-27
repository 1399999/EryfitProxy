

using System;

namespace EryfitProxy.Kernel.Rules
{
    /// <summary>
    ///     Put this attribute on a filter property to include the value to the unique hash id generation.
    /// </summary>
    public class FilterDistinctiveAttribute : PropertyDistinctiveAttribute, IVariableHolder
    {
        
    }
}

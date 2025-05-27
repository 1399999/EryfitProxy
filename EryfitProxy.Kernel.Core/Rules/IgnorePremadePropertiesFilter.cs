

using System;
using System.Collections.Generic;
using System.Linq;
using EryfitProxy.Kernel.Rules.Filters;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.TypeInspectors;

namespace EryfitProxy.Kernel.Rules
{
    internal class IgnorePremadePropertiesFilter : TypeInspectorSkeleton
    {
        private readonly ITypeInspector _innerTypeInspector;

        public IgnorePremadePropertiesFilter(ITypeInspector innerTypeInspector)
        {
            _innerTypeInspector = innerTypeInspector;
        }

        public override IEnumerable<IPropertyDescriptor> GetProperties(Type type, object? container)
        {
            var properties = _innerTypeInspector.GetProperties(type, container);

            if (!type.IsSubclassOf(typeof(Filter))) {
                return properties;
            }

            var constructor = type.GetConstructor(Type.EmptyTypes);

            if (constructor == null) {
                return properties;
            }

            var filter = constructor.Invoke(null) as Filter;

            if (filter == null || !filter.PreMadeFilter) {
                return properties;
            }
            
            // Retains only inverted 

            return properties.Where(p =>
                p.Name.Equals(nameof(Filter.TypeKind), 
                    StringComparison.OrdinalIgnoreCase));
        }
    }
}

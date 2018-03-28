using System;
using System.Reflection;

namespace EasyConstructor
{
    public static class PropertyInfoExtensions
    {
        public static bool IsUsableFor(this PropertyInfo property, ParameterInfo parameter)
        {
            //TODO I don't like checking for anonymous type here
            return property.Name.Equals(parameter.Name, StringComparison.OrdinalIgnoreCase) &&
                (parameter.ParameterType.IsAssignableFrom(property.PropertyType) ||
                    property.PropertyType.Name.Contains("AnonymousType"));
        }

        public static bool IsAnonymousType(this PropertyInfo property)
        {
            return property.PropertyType.Name.Contains("AnonymousType");
        }
    }
}
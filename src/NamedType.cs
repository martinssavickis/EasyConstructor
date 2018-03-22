using System;

namespace EasyConstructor
{
    public class NamedType
    {
        public Type ParamType { get; private set; }
        public String Name { get; private set; }

        public NamedType(Type paramType, string name)
        {
            ParamType = paramType;
            Name = name;
        }

        public bool IsAssignableTo(NamedType other)
        {
            return this.Name.Equals(other.Name, StringComparison.OrdinalIgnoreCase) && //only if names match
                (other.ParamType.IsAssignableFrom(this.ParamType) || //if same/base class or interface
                this.ParamType.Name.Contains("AnonymousType")); // try to create from anonymous
        }

    }
}
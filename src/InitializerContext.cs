using System;
using System.Collections.Generic;
using System.Reflection;

namespace EasyConstructor
{
    public class InitializerContext
    {
        public Dictionary<Type, Dictionary<string, object>> DefaultsLookup { get; } = new Dictionary<Type, Dictionary<string, object>>();
        public Dictionary<Type, ConstructorInfo> ConstructorLookup { get; } = new Dictionary<Type, ConstructorInfo>();
        public Dictionary<Type, Type> InterfaceLookup { get; } = new Dictionary<Type, Type>();
    }
}
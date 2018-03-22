using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EasyConstructor
{
    public class Initializer
    {
        private InitializerContext context = new InitializerContext();

        public void AddDefaultValues<T>(object values) where T : class
        {
            var type = typeof(T);
            AddDefaultValues(type, values);
        }

        public void RegisterInterface<TInterface, TConcrete>()
        {
            var interfaceType = typeof(TInterface);
            if (!interfaceType.IsInterface)
            {
                throw new TypeLoadException("must be interface");
            }

            var concreteType = typeof(TConcrete);
            if (concreteType.IsInterface)
            {
                throw new TypeLoadException("must be concrete class");
            }

            context.InterfaceLookup[interfaceType] = concreteType;
        }

        public void UseConstructor<T>(Func<ConstructorInfo[], ConstructorInfo> select)
        {
            var type = typeof(T);
            context.ConstructorLookup[type] = select(type.GetConstructors());
        }

        public T Create<T>(object values = null) where T : class
        {
            var type = typeof(T);
            return (T) CreateWithValues(type, values);
        }

        private void AddDefaultValues(Type type, object values)
        {
            Dictionary<string, object> classLookup;
            if (!context.DefaultsLookup.TryGetValue(type, out classLookup))
            {
                classLookup = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
                context.DefaultsLookup.Add(type, classLookup);
            }

            var props = values.GetType().GetProperties();
            foreach (var prop in props)
            {
                classLookup[prop.Name] = prop.GetValue(values);
            }
        }

        private object CreateWithValues(Type type, object values)
        {
            var constructorParams = GetApplicableConstructor(type, values).GetParameters();

            var resolvedParams = new List<object>(constructorParams.Length);
            foreach (var constructorParam in constructorParams)
            {

                resolvedParams.Add(ResolveConstructorParameter(type, constructorParam, values));
            }

            return Activator.CreateInstance(type, resolvedParams.ToArray());
        }

        private object ResolveConstructorParameter(Type type, ParameterInfo parameterInfo, object values)
        {
            NamedType constructorParameter = new NamedType(parameterInfo.ParameterType, parameterInfo.Name);
            if (values != null)
            {
                object fromParameters = ResolveFromValues(parameterInfo, values);
                if (fromParameters != null)
                {
                    return fromParameters;
                }
            }

            return ResolveFromDefaults(type, parameterInfo);
        }

        private Object ResolveFromValues(ParameterInfo parameterInfo, object values)
        {
            NamedType constructorParameter = new NamedType(parameterInfo.ParameterType, parameterInfo.Name);
            var paramList = values.GetType().GetProperties().Select(p =>(NamedType: new NamedType(p.PropertyType, p.Name), PropertyInfo : p));

            //check if there is a parameter in anonymous object we can use
            var found = paramList.SingleOrDefault(p => p.NamedType.IsAssignableTo(constructorParameter));
            if (found.NamedType != null)
            {
                Type typeToCreate = ResolveType(constructorParameter.ParamType, found.NamedType.ParamType);

                //parameter is object that needs creating
                if (found.NamedType.ParamType.Name.ToString().Contains("AnonymousType"))
                {
                    return CreateWithValues(typeToCreate, found.PropertyInfo.GetValue(values));
                }

                if (typeToCreate.IsAssignableFrom(found.NamedType.ParamType))
                {
                    return found.PropertyInfo.GetValue(values);
                }
            }
            return null;
        }

        private Object ResolveFromDefaults(Type type, ParameterInfo parameterInfo)
        {
            if (context.DefaultsLookup.ContainsKey(type) && context.DefaultsLookup[type].ContainsKey(parameterInfo.Name))
            {
                var found = context.DefaultsLookup[type][parameterInfo.Name];
                if (found.GetType().Name.ToString().Contains("AnonymousType"))
                {
                    return CreateWithValues(parameterInfo.ParameterType, found);
                }
                return found;
            }

            if (parameterInfo.ParameterType.IsValueType)
            {
                return Activator.CreateInstance(parameterInfo.ParameterType);
            }

            return null;
        }

        private Type ResolveType(Type constructorType, Type foundType)
        {
            //if constructor param is happy with found type
            if (constructorType.IsAssignableFrom(foundType))
            {
                return foundType;
            }

            //if constructor param is interface we have registered, return registered
            if (constructorType.IsInterface && context.InterfaceLookup.ContainsKey(constructorType))
            {
                return context.InterfaceLookup[constructorType];
            }

            // we havent found type to use, so return initial
            return constructorType;
        }

        private ConstructorInfo GetApplicableConstructor(Type type, object values)
        {
            if (context.ConstructorLookup.ContainsKey(type))
            {
                return context.ConstructorLookup[type];
            }

            //TODO check if class with no declared constructors will have default in list
            ConstructorInfo bestConstructor = type.GetConstructors().First();
            if (values != null)
            {
                //naive implementation, seems sound tho

                var props = values.GetType().GetProperties().Select(p => new NamedType(p.PropertyType, p.Name)).ToList();
                int bestMatches = 0;
                int bestParamCount = int.MaxValue;
                foreach (var constructor in type.GetConstructors())
                {
                    var constructorParams = constructor.GetParameters().Select(p => new NamedType(p.ParameterType, p.Name));
                    var matched = constructorParams.Count(param => props.Any(prop => prop.IsAssignableTo(param)));

                    if (matched > bestMatches || (matched == bestMatches && constructorParams.Count() < bestParamCount))
                    {
                        bestMatches = matched;
                        bestParamCount = constructorParams.Count();
                        bestConstructor = constructor;
                    }
                }
            }

            return bestConstructor;
        }
    }
}
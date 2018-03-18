using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EasyConstructor
{
    public class Initializer
    {
        private Dictionary<Type, Dictionary<string, object>> defaultsLookup = new Dictionary<Type, Dictionary<string, object>>();
        private Dictionary<Type, ConstructorInfo> constructorLookup = new Dictionary<Type, ConstructorInfo>();

        public void AddDefaultParameters<T>(object parameters) where T : class
        {
            var type = typeof(T);
            AddDefaultParameters(type, parameters);
        }

        public void UseConstructor<T>(Func<ConstructorInfo[], ConstructorInfo> select)
        {
            var type = typeof(T);
            constructorLookup[type] = select(type.GetConstructors());
        }

        public T Create<T>(object parameters = null) where T : class
        {
            var type = typeof(T);
            return (T) CreateWithParameters(type, parameters);
        }

        private void AddDefaultParameters(Type type, object parameters)
        {
            Dictionary<string, object> classLookup;
            if (!defaultsLookup.TryGetValue(type, out classLookup))
            {
                classLookup = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
                defaultsLookup.Add(type, classLookup);
            }

            var props = parameters.GetType().GetProperties();
            foreach (var prop in props)
            {
                //nested parameter
                //TODO check that this can only be done to reference types
                if (prop.GetType().ToString().Contains("AnonymousType"))
                {
                    AddDefaultParameters(prop.PropertyType, prop.GetValue(parameters));
                }
                classLookup[prop.Name] = prop.GetValue(parameters);
            }
        }

        private object CreateWithParameters(Type type, object parameters)
        {
            var constructorParams = GetApplicableConstructor(type, parameters).GetParameters();

            var resolvedParams = new List<object>(constructorParams.Length);
            foreach (var constructorParam in constructorParams)
            {
                resolvedParams.Add(ResolveConstructorParameter(type, constructorParam, parameters));
            }

            return Activator.CreateInstance(type, resolvedParams.ToArray());
        }

        private object ResolveConstructorParameter(Type type, ParameterInfo parameterInfo, object parameters)
        {
            if (parameters != null)
            {
                var paramList = parameters.GetType().GetProperties();
                var found = paramList.SingleOrDefault(p => p.Name.Equals(parameterInfo.Name, StringComparison.OrdinalIgnoreCase));
                if (found != null)
                {
                    //parameter is object that needs creating
                    if (found.Name.ToString().Contains("AnonymousType"))
                    {
                        //TODO call create with parameters here
                        return null;
                    }

                    if (found.PropertyType.Equals(parameterInfo.ParameterType))
                    {
                        return found.GetValue(parameters);
                    }
                }
            }

            if (defaultsLookup.ContainsKey(type) && defaultsLookup[type].ContainsKey(parameterInfo.Name))
            {
                var found = defaultsLookup[type][parameterInfo.Name];
                if (found.GetType().Name.ToString().Contains("AnonymousType"))
                {
                    //TODO call create with parameters here
                    return null;
                }
                return found;
            }

            if (parameterInfo.ParameterType.IsValueType)
            {
                return Activator.CreateInstance(parameterInfo.ParameterType);
            }

            return null;
        }

        private ConstructorInfo GetApplicableConstructor(Type type, object parameters)
        {
            if (constructorLookup.ContainsKey(type))
            {
                return constructorLookup[type];
            }

            //TODO check if class with no declared constructors will have default in list
            ConstructorInfo bestConstructor = type.GetConstructors().First();
            if (parameters != null)
            {
                //naive implementation, seems sound tho
                var props = parameters.GetType().GetProperties().Select(p =>(p.PropertyType, p.Name.ToUpper())).ToList();
                int bestMatches = 0;
                int bestParamCount = int.MaxValue;
                foreach (var constructor in type.GetConstructors())
                {
                    var constructorParams = constructor.GetParameters().Select(p =>(p.ParameterType, p.Name.ToUpper()));
                    var matched = constructorParams.Count(p => props.Contains(p));

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
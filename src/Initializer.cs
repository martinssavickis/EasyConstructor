using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EasyConstructor
{
    public class Initializer
    {
        private Dictionary<Type, Dictionary<string, object>> defaultsLookup = new Dictionary<Type, Dictionary<string, object>>();

        public void AddDefaultParameters<T>(object parameters) where T : class
        {
            var type = typeof(T);
            AddDefaultParameters(type, parameters);
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
            var constructorParams = GetApplicableConstructor(type).GetParameters();

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

        //TODO decide on sensible default
        //add config to pass in constructor selector delegate
        private ConstructorInfo GetApplicableConstructor(Type type)
        {
            var constructors = type.GetConstructors();

            return constructors.First();
        }
    }
}
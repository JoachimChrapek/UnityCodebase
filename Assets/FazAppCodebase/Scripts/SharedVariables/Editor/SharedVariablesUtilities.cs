using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace FazApp.SharedVariables.Editor
{
    public static class SharedVariablesUtilities
    {
        public static IEnumerable<Type> GetSharedVariablesTypeCollection()
        {
            return TypeCache.GetTypesDerivedFrom<SharedVariable>().Where(t => t.IsAbstract == false && t.IsInterface == false);
        }
        
        public static Type GetSharedVariableValueType(Type sharedVariableType)
        {
            return sharedVariableType.GetInterfaces().FirstOrDefault(it => it.IsGenericType && it.GetGenericTypeDefinition() == typeof(ISharedVariable<>))?.GenericTypeArguments.FirstOrDefault();
        }
    }
}

using System;
using System.Linq;

namespace FazApp.SharedVariables.Unity.Editor
{
    public static class SharedVariablesUtilities
    {
        public static Type GetSharedVariableValueType(Type sharedVariableType)
        {
            return sharedVariableType.GetInterfaces().FirstOrDefault(it => it.IsGenericType && it.GetGenericTypeDefinition() == typeof(ISharedVariable<>)).GenericTypeArguments.FirstOrDefault();
        }
    }
}

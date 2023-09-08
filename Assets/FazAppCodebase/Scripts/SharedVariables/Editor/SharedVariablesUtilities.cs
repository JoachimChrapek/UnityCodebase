using System;
using System.Linq;

namespace FazApp.SharedVariables.Editor
{
    public static class SharedVariablesUtilities
    {
        public static Type GetSharedVariableValueType(Type sharedVariableType)
        {
            return sharedVariableType.GetInterfaces().FirstOrDefault(it => it.IsGenericType && it.GetGenericTypeDefinition() == typeof(ISharedVariable<>))?.GenericTypeArguments.FirstOrDefault();
        }
    }
}

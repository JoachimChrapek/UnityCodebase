using System;

namespace FazApp.SharedVariables
{
    public static class SV
    {
        private static readonly SharedVariablesContainer Container = new();

        public static T Get<T>() where T : SharedVariable, new()
        {
            return Container.Get<T>();
        }

        public static bool TryGet(Type sharedVariableType, out ISharedVariable sharedVariable)
        {
            return Container.TryGet(sharedVariableType, out sharedVariable);
        }
        
        public static bool TryGet<TSharedVariableValue>(Type sharedVariableType, out ISharedVariable<TSharedVariableValue> sharedVariable)
        {
            return Container.TryGet<TSharedVariableValue>(sharedVariableType, out sharedVariable);
        }
    }
}

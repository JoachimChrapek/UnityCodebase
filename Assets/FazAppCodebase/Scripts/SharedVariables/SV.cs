using FazApp.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FazApp.SharedVariables
{
    //TODO make SV public (as API), create internal SharedVariablesContainer with dictionary. Move TryGet methods there
    public static class SV
    {
        private static Dictionary<string, ISharedVariable> CachedSharedVariables = new();

        public static T Get<T>() where T : SharedVariable, new()
        {
            string key = typeof(T).FullName;

            if (CachedSharedVariables.TryGetValue(key, out ISharedVariable sharedVariable) == false)
            {
                T newSharedVariable = new ();
                newSharedVariable.Initialize();
                CachedSharedVariables.Add(key, newSharedVariable);
                
                sharedVariable = newSharedVariable;
            }

            return (T)sharedVariable;
        }

        public static bool TryGet(Type sharedVariableType, out ISharedVariable sharedVariable)
        {
            if (!sharedVariableType.GetInterfaces().Contains(typeof(ISharedVariable)))
            {
                Log.Error($"Type {sharedVariableType} does not implement {nameof(ISharedVariable)} interface");

                sharedVariable = default;
                return false;
            }
            
            string key = sharedVariableType.FullName;
            
            if (CachedSharedVariables.TryGetValue(key, out sharedVariable) == false)
            {
                SharedVariable newSharedVariable = Activator.CreateInstance(sharedVariableType) as SharedVariable;
                newSharedVariable.Initialize();

                sharedVariable = newSharedVariable;
                CachedSharedVariables.Add(key, sharedVariable);
            }

            return sharedVariable != null;
        }
        
        public static bool TryGet<TSharedVariableValue>(Type sharedVariableType, out ISharedVariable<TSharedVariableValue> sharedVariable)
        {
            if (!sharedVariableType.GetInterfaces().Contains(typeof(ISharedVariable)))
            {
                Log.Error($"Type {sharedVariableType} does not implement {nameof(ISharedVariable)} interface");

                sharedVariable = default;
                return false;
            }
            
            string key = sharedVariableType.FullName;
            
            if (CachedSharedVariables.TryGetValue(key, out ISharedVariable sharedVariableBase) == false)
            {
                SharedVariable<TSharedVariableValue> newSharedVariable = Activator.CreateInstance(sharedVariableType) as SharedVariable<TSharedVariableValue>;
                newSharedVariable.Initialize();
                
                sharedVariable = newSharedVariable;
                CachedSharedVariables.Add(key, sharedVariable);
            }
            else
            {
                sharedVariable = sharedVariableBase as ISharedVariable<TSharedVariableValue>;
            }

            return sharedVariable != null;
        }
    }
}

using System;

namespace FazApp.SharedVariables.Unity.Editor
{
    [Serializable]
    public class SharedVariableTypeData
    {
        public Type SharedVariableType { get; }
        public SharedVariableScriptableObject ScriptableObjectInstance { get; }
        public Type ShredVariableValueType { get; }
        public bool HaveScriptableObjectType { get; }

        public SharedVariableTypeData(Type sharedVariableType, SharedVariableScriptableObject scriptableObjectInstance, Type shredVariableValueType, bool haveScriptableObjectType)
        {
            SharedVariableType = sharedVariableType;
            ScriptableObjectInstance = scriptableObjectInstance;
            ShredVariableValueType = shredVariableValueType;
            HaveScriptableObjectType = haveScriptableObjectType;
        }
    }
}

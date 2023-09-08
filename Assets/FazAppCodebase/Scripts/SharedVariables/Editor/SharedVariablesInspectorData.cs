using System;
using System.Collections.Generic;

namespace FazApp.SharedVariables.Editor
{
    public class SharedVariablesInspectorData
    {
        public List<SharedVariableTypeData> SharedVariablesTypeDataCollection { get; } = new();
        public Dictionary<Type, Type> ValueTypeToSharedVariableScriptableObjectTypeMap { get; } = new();
    }
}

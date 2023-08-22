using System;
using System.Collections.Generic;

namespace FazApp.SharedVariables.Unity.Editor
{
    public class SharedVariablesInspectorData
    {
        public List<SharedVariableTypeData> SharedVariablesTypeDataCollection { get; } = new();
        public Dictionary<Type, Type> ValueTypeToSharedVariableScriptableObjectTypeMap { get; } = new();
        
        public string SearchText { get; set; }
    }
}

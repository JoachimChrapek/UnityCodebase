using System;
using System.Collections.Generic;
using UnityEngine;

namespace FazApp.SharedVariables.Editor
{
    public class RuntimeSharedVariableInspectorDataContainer : ScriptableObject
    {
        [field: SerializeField]
        public List<RuntimeSharedVariableInspectorData> SharedVariablesCollection { get; private set; } = new();

        internal void InitializeSharedVariables()
        {
            foreach (RuntimeSharedVariableInspectorData sharedVariableWrapper in SharedVariablesCollection)
            {
                sharedVariableWrapper.CachedSharedVariable.EditorInitialization();
            }
        }

        internal void DecomissionSharedVariables()
        {
            foreach (RuntimeSharedVariableInspectorData sharedVariableWrapper in SharedVariablesCollection)
            {
                sharedVariableWrapper.CachedSharedVariable.EditorDecomission();
            }
        }

        private void OnDestroy()
        {
            DecomissionSharedVariables();
        }
    }

    [Serializable]
    public class RuntimeSharedVariableInspectorData
    {
        [field: SerializeReference]
        public SharedVariable CachedSharedVariable { get; private set; }

        public bool IsAutoSaveEnabled { get; set; }
        
        public RuntimeSharedVariableInspectorData(SharedVariable sharedVariable)
        {
            CachedSharedVariable = sharedVariable;
        }
    }
}

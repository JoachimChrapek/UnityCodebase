using System;
using System.Collections.Generic;
using FazApp.SharedVariables;
using UnityEngine;

namespace FazApp
{
    public class SharedVariableContainer : ScriptableObject
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

            //TODO remove
            Debug.Log("DecomissionSharedVariables");
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

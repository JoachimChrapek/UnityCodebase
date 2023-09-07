using FazApp.Core;
using System;
using UnityEngine;

namespace FazApp.SharedVariables
{
    public abstract class SharedVariableScriptableObject : ScriptableObject
    {
        [field: SerializeField, ReadOnly]
        public string AssignedSharedVariableTypeName { get; private set; }
        
        public void SetAssignedSharedVariableTypeName(Type assignedSharedVariableType)
        {
            AssignedSharedVariableTypeName = assignedSharedVariableType.AssemblyQualifiedName;
        }
    }
    
    public abstract class SharedVariableScriptableObject<T> : SharedVariableScriptableObject, ISharedVariable<T>
    {
        private Type assignedSharedVariableType;
        
        public event Action<T> valueChanged
        {
            add => AssignedSharedVariable.valueChanged += value;
            remove => AssignedSharedVariable.valueChanged -= value;
        }

        public T Value { 
            get => AssignedSharedVariable.Value;
            set => AssignedSharedVariable.Value = value;
        }
        
        private ISharedVariable<T> assignedSharedVariable;

        private ISharedVariable<T> AssignedSharedVariable
        {
            get
            {
                if (assignedSharedVariable == null)
                {
                    Initialize();
                }

                return assignedSharedVariable;
            }
        }

        private void Initialize()
        {
            if (assignedSharedVariable != null)
            {
                return;
            }
            
            assignedSharedVariableType = GetAssignedSharedVariableType();

            if (!SV.TryGet(assignedSharedVariableType, out assignedSharedVariable))
            {
                //TODO use logger
                Debug.LogError("Couldn't get assigned shared variable during initialization\n" +
                               $"Scriptable object: {name}\n" +
                               $"Assigned Shared Variable type string: {assignedSharedVariableType}");
            }
        }
        
        private Type GetAssignedSharedVariableType()
        {
            return Type.GetType(AssignedSharedVariableTypeName);
        }
    }
}

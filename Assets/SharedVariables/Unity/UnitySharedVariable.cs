using System;
using UnityEngine;

namespace FazApp.SharedVariables
{
#if UNITY_EDITOR
    public partial class SharedVariable
    {
        internal event Action editorValueChanged;
        
        //TODO better name
        internal abstract void EditorInitialization();
        //TODO better name
        internal abstract void EditorDecomission();
        
        internal abstract void UpdateEditorValue();
        internal abstract void UpdateValueAfterEditorChange();
        internal abstract void ForceNotifyValueChanged();

        protected void NotifyEditorValueChanged()
        {
            editorValueChanged?.Invoke();
        }
    }
    
    public partial class SharedVariable<T>
    {
        [field: SerializeField]
        internal T EditorValue { get; set; }

        //TODO better name
        private bool isEditorInitialized;
        
        internal override void EditorInitialization()
        {
            if (isEditorInitialized)
            {
                return;
            }
            
            valueChanged += EditorInternalOnValueChange;
            UpdateEditorValue();
            
            isEditorInitialized = true;
        }

        internal override void EditorDecomission()
        {
            if (!isEditorInitialized)
            {
                return;
            }
            
            valueChanged -= EditorInternalOnValueChange;
            isEditorInitialized = false;
        }

        internal override void UpdateEditorValue()
        {
            EditorValue = value;
            NotifyEditorValueChanged();
        }

        internal override void UpdateValueAfterEditorChange()
        {
            Value = EditorValue;
        }

        internal override void ForceNotifyValueChanged()
        {
            NotifyValueChanged();
        }

        private void EditorInternalOnValueChange(T newValue)
        {
            UpdateEditorValue();
        }
    }
#endif
}

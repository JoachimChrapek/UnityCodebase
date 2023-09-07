using FazApp.EditorExtensions.Editor;
using System;
using System.Collections.Generic;

namespace FazApp.SharedVariables.Unity.Editor
{
    public class SharedVariablesInspector
    {
        public event Action reinitializeRequested;
        public event Action refreshRequested;
        
        protected SharedVariablesInspectorData InspectorData { get; private set; }
        protected List<SharedVariableScriptableObject> ScriptableObjectsCollection { get; private set; }

        private Action delayedButtonAction;
        
        public void Initialize(SharedVariablesInspectorData inspectorData, List<SharedVariableScriptableObject> scriptableObjectsCollection)
        {
            InspectorData = inspectorData;
            ScriptableObjectsCollection = scriptableObjectsCollection;
        }
        
        public virtual void DrawInspector()
        {
            TryInvokeDelayedButtonAction();
        }

        protected void DrawDelayedButton(string buttonText, Action delayedAction)
        {
            ExtendedGUI.DrawButton(buttonText, () => delayedButtonAction = delayedAction);
        }
        
        protected void RefreshInspectorWindow()
        {
            refreshRequested?.Invoke();
        }
        
        protected void ReinitializeInspectorWindow()
        {
            reinitializeRequested?.Invoke();
        }

        protected virtual bool CanDrawSharedVariable(Type sharedVariableType)
        {
            return string.IsNullOrEmpty(InspectorData.SearchText) || sharedVariableType.FullName.Contains(InspectorData.SearchText, StringComparison.InvariantCultureIgnoreCase);
        }
        
        private void TryInvokeDelayedButtonAction()
        {
            if (delayedButtonAction != null)
            {
                delayedButtonAction.Invoke();
                delayedButtonAction = null;
            }
        }

    }
}

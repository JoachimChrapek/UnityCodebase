using FazApp.EditorExtensions.Editor;
using System;

namespace FazApp.SharedVariables.Editor
{
    public class SharedVariablesInspector
    {
        public event Action reinitializeRequested;
        public event Action refreshRequested;
        
        protected SharedVariablesInspectorData InspectorData { get; private set; }
        private SearchBarController searchBarController;
        
        private Action delayedButtonAction;
        
        public void Initialize(SharedVariablesInspectorData inspectorData, SearchBarController searchBarController)
        {
            InspectorData = inspectorData;
            this.searchBarController = searchBarController;
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
            return string.IsNullOrEmpty(searchBarController.SearchText) || sharedVariableType.FullName.Contains(searchBarController.SearchText, StringComparison.InvariantCultureIgnoreCase);
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

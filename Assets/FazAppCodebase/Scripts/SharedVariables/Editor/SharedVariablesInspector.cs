using FazApp.EditorExtensions.Editor;
using System;

namespace FazApp.SharedVariables.Editor
{
    public abstract class SharedVariablesInspector
    {
        public event Action reinitializeRequested;
        
        protected SharedVariablesInspectorData InspectorData { get; private set; }
        private SearchBarController searchBarController;
        
        private bool reinitializationRequestedFlag;
        
        public void Initialize(SharedVariablesInspectorData inspectorData, SearchBarController searchBarController)
        {
            InspectorData = inspectorData;
            this.searchBarController = searchBarController;
        }
        
        public void DrawInspector()
        {
            DrawInspectorContent();
            TryReinitializeInspectorWindow();
        }

        protected abstract void DrawInspectorContent();

        protected void ReinitializeInspectorWindow()
        {
            reinitializationRequestedFlag = true;
        }

        protected virtual bool CanDrawSharedVariable(Type sharedVariableType)
        {
            return string.IsNullOrEmpty(searchBarController.SearchText) || sharedVariableType.FullName.Contains(searchBarController.SearchText, StringComparison.InvariantCultureIgnoreCase);
        }
        
        private void TryReinitializeInspectorWindow()
        {
            if (!reinitializationRequestedFlag)
            {
                return;
            }

            reinitializationRequestedFlag = false;
            reinitializeRequested?.Invoke();
        }

    }
}

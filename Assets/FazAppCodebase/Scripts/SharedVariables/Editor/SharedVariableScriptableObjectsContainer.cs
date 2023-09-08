using System;
using System.Collections.Generic;
using System.Linq;

namespace FazApp.SharedVariables.Editor
{
    public class SharedVariableScriptableObjectsContainer
    {
        private List<SharedVariableScriptableObject> scriptableObjectsCollection;

        public SharedVariableScriptableObjectsContainer()
        {
            RefreshScriptableObjectsCollection();
        }

        public void RefreshScriptableObjectsCollection()
        {
            scriptableObjectsCollection = SharedVariableScriptableObjectsLoader.Load();
        }
        
        public SharedVariableScriptableObject GetSharedVariableScriptableObject(Type sharedVariableType)
        {
            string sharedVariableTypeName = sharedVariableType.AssemblyQualifiedName;
            return scriptableObjectsCollection.FirstOrDefault(sv => sv.AssignedSharedVariableTypeName == sharedVariableTypeName);
        }
    }
}

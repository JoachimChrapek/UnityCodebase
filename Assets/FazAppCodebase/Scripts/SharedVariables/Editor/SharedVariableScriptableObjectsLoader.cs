using System.Collections.Generic;
using UnityEditor;

namespace FazApp.SharedVariables.Editor
{
    public class SharedVariableScriptableObjectsLoader
    {
        public static List<SharedVariableScriptableObject> Load()
        {
            List<SharedVariableScriptableObject> collection = new();
            List<string> pathsCollection = GetAllSharedVariableScriptableObjectsPathsCollection();

            foreach (string path in pathsCollection)
            {
                SharedVariableScriptableObject scriptableObject = AssetDatabase.LoadAssetAtPath<SharedVariableScriptableObject>(path);
                collection.Add(scriptableObject);
            }
        
            return collection;
        }
        
        private static List<string> GetAllSharedVariableScriptableObjectsPathsCollection()
        {
            List<string> pathsCollection = new();
            string[] guidsCollection = AssetDatabase.FindAssets($"t: {nameof(SharedVariableScriptableObject)}");
        
            foreach (string guid in guidsCollection)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                pathsCollection.Add(path);
            }

            return pathsCollection;
        }
    }
}

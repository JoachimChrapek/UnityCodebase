// using System;
// using System.Collections.Generic;
// using System.Linq;
// using UnityEditor;
// using UnityEngine;
//
// namespace FazApp.SharedVariables.Unity
// {
//     [CreateAssetMenu(fileName = nameof(SharedVariableScriptableObjectsContainer), menuName = "FazApp/Scriptable Objects/" + nameof(SharedVariableScriptableObjectsContainer))]
//     public class SharedVariableScriptableObjectsContainer : ScriptableObject
//     {
//         [SerializeField]
//         private List<SharedVariableScriptableObject> scriptableObjectsCollection;
//
//         // public void InitializeSharedVariableScriptableObjects()
//         // {
//         //     foreach (SharedVariableScriptableObject sharedVariableScriptableObject in scriptableObjectsCollection)
//         //     {
//         //         sharedVariableScriptableObject.Initialize();
//         //     }
//         // }
//         
//         // public SharedVariableScriptableObject Get(Type sharedVariableType)
//         // {
//         //     string sharedVariableTypeName = sharedVariableType.AssemblyQualifiedName;
//         //     return scriptableObjectsCollection.FirstOrDefault(sv => sv.AssignedSharedVariableTypeName == sharedVariableTypeName);
//         // }
//         //
//         // public void Add(SharedVariableScriptableObject sharedVariableScriptableObject)
//         // {
//         //     if (!scriptableObjectsCollection.Contains(sharedVariableScriptableObject))
//         //     {
//         //         scriptableObjectsCollection.Add(sharedVariableScriptableObject);
//         //     }
//         // }
//         
// //         public void ClearEmptyScriptableObjects()
// //         {
// //             int removedItems = scriptableObjectsCollection.RemoveAll(so => so == null);
// //
// // #if UNITY_EDITOR
// //             if (removedItems > 0)
// //             {
// //                 EditorUtility.SetDirty(this);
// //                 AssetDatabase.SaveAssets();
// //                 AssetDatabase.Refresh();
// //             }
// // #endif
// //         }
//     }
// }

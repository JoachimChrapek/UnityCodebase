using FazApp.EditorExtensions.Editor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace FazApp.SharedVariables.Unity.Editor
{
    public class SharedVariablesInspectorWindow : ExtendedEditorWindow
    {
        private const string WindowName = "Shared Variables Inspector";

        //private SharedVariableScriptableObjectsContainer scriptableObjectsContainer;
        private List<SharedVariableScriptableObject> scriptableObjectsCollection;
        private SharedVariablesInspectorData inspectorData;
        
        private EditorSharedVariablesInspector editorInspector;
        private RuntimeSharedVariablesInspector runtimeInspector;

        private string searchText;
        private Vector2 scrollPosition;
        
        [MenuItem(EditorValues.MenuItemRoot + WindowName)]
        private static void ShowWindow()
        {
            SharedVariablesInspectorWindow windowInstance = GetWindow<SharedVariablesInspectorWindow>(WindowName);
            windowInstance.Initialize();
        }

        protected void OnGUI()
        {
            if (!IsSharedVariableScriptableObjectsCollectionLoaded())
            {
                LoadSharedVariableScriptableObjectsCollection();
            }
            
            // if (!IsSharedVariableScriptableObjectsCollectionLoaded())
            // {
            //     DrawSharedVariableScriptableObjectsContainerNotLoadedInfo();
            //     return;
            // }
            
            if (!IsInitialized())
            {
                Initialize();
            }
 
            DrawInspector();
        }

        
        
        private void OnDestroy()
        {
            //TODO decomission RuntimeInspector
        }

        private void DrawInspector()
        {
            //DrawSharedVariableScriptableObjectsContainerInfo();
            DrawRefreshButton();

            GUILayout.Space(10);
            
            DrawSearchBar();
            
            GUILayout.Space(10);
            
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            
            if (Application.isPlaying)
            {
                runtimeInspector.DrawInspector();
            }
            else
            {
                editorInspector.DrawInspector();
            }
            
            EditorGUILayout.EndScrollView();
        }

        // private void DrawSharedVariableScriptableObjectsContainerNotLoadedInfo()
        // {
        //     StringBuilder messageBuilder = new();
        //     List<string> paths = SharedVariableScriptableObjectsLoader.GetAllSharedVariableScriptableObjectsContainerPathsCollection();
        //
        //     if (paths.Count == 0)
        //     {
        //         messageBuilder.AppendLine($"Couldn't find object of type {nameof(SharedVariableScriptableObjectsContainer)} in Assets folder");
        //     }
        //     else
        //     {
        //         messageBuilder.AppendLine($"Found multiple objects of type {nameof(SharedVariableScriptableObjectsContainer)} in Assets folder");
        //
        //         foreach (string path in paths)
        //         {
        //             messageBuilder.AppendLine(path);
        //         }
        //     }
        //     
        //     EditorGUILayout.HelpBox(messageBuilder.ToString(), MessageType.Error);
        // }
        
        // private void DrawSharedVariableScriptableObjectsContainerInfo()
        // {
        //     GUI.enabled = false;
        //     EditorGUILayout.ObjectField(nameof(SharedVariableScriptableObjectsContainer), scriptableObjectsContainer, typeof(SharedVariableScriptableObjectsContainer), false);
        //     GUI.enabled = true;
        // }
        
        private void DrawRefreshButton()
        {
            ExtendedGUI.DrawButton("Refresh", RefreshWindow);
        }

        private void DrawSearchBar()
        {
            inspectorData.SearchText = EditorGUILayout.TextField("Search", inspectorData.SearchText);
        }

        private bool IsSharedVariableScriptableObjectsCollectionLoaded()
        {
            return scriptableObjectsCollection != null;
        }

        private void LoadSharedVariableScriptableObjectsCollection()
        {
            scriptableObjectsCollection = SharedVariableScriptableObjectsLoader.Load();
        }
        
        private bool IsInitialized()
        {
            return inspectorData != null && editorInspector != null && runtimeInspector != null;
        }
        
        private void Initialize()
        {
            if (scriptableObjectsCollection == null)
            {
                return;
            }
            
            //scriptableObjectsContainer.ClearEmptyScriptableObjects();
            inspectorData = new SharedVariablesInspectorData();
            
            editorInspector = InitializeInspector<EditorSharedVariablesInspector>();
            runtimeInspector = InitializeInspector<RuntimeSharedVariablesInspector>();
            
            RefreshSharedVariablesTypesCollection();
        }

        private T InitializeInspector<T>() where T : SharedVariablesInspector, new()
        {
            T inspector = new ();
            inspector.Initialize(inspectorData, scriptableObjectsCollection);

            inspector.reinitializeRequested += ReinitializeWindow;
            inspector.refreshRequested += RefreshWindow;

            return inspector;
        }

        private void ReinitializeWindow()
        {
            LoadSharedVariableScriptableObjectsCollection();
            RefreshSharedVariablesTypesCollection();
            RefreshWindow();
        }
        
        private void RefreshWindow()
        {
            Repaint();
        }
        
        private void RefreshSharedVariablesTypesCollection()
        {
            RefreshValueTypeToSharedVariableScriptableObjectTypeMap();
            
            inspectorData.SharedVariablesTypeDataCollection.Clear();
            IEnumerable<Type> sharedVariablesTypeCollection = GetSharedVariablesTypeCollection();
            
            foreach (Type sharedVariableType in sharedVariablesTypeCollection)
            {
                SharedVariableScriptableObject scriptableObjectInstance = GetSharedVariableScriptableObject(sharedVariableType);
                Type sharedVariableValueType = SharedVariablesUtilities.GetSharedVariableValueType(sharedVariableType);
                bool haveScriptableObjectType = inspectorData.ValueTypeToSharedVariableScriptableObjectTypeMap.ContainsKey(sharedVariableValueType);
                SharedVariableTypeData sharedVariableData = new (sharedVariableType, scriptableObjectInstance, sharedVariableValueType, haveScriptableObjectType);
                inspectorData.SharedVariablesTypeDataCollection.Add(sharedVariableData);
            }
        }

        private void RefreshValueTypeToSharedVariableScriptableObjectTypeMap()
        {
            inspectorData.ValueTypeToSharedVariableScriptableObjectTypeMap.Clear();
            IEnumerable<Type> sharedVariableScriptableObjectTypesCollection = TypeCache.GetTypesDerivedFrom<SharedVariableScriptableObject>().Where(t => t.IsAbstract == false);

            foreach (Type sharedVariableScriptableObjectType in sharedVariableScriptableObjectTypesCollection)
            {
                Type valueType = SharedVariablesUtilities.GetSharedVariableValueType(sharedVariableScriptableObjectType);
                inspectorData.ValueTypeToSharedVariableScriptableObjectTypeMap.Add(valueType, sharedVariableScriptableObjectType);
            }
        }
        
        private IEnumerable<Type> GetSharedVariablesTypeCollection()
        {
            return TypeCache.GetTypesDerivedFrom<SharedVariable>().Where(t => t.IsAbstract == false && t.IsInterface == false);
        }
        
        private SharedVariableScriptableObject GetSharedVariableScriptableObject(Type sharedVariableType)
        {
            string sharedVariableTypeName = sharedVariableType.AssemblyQualifiedName;
            return scriptableObjectsCollection.FirstOrDefault(sv => sv.AssignedSharedVariableTypeName == sharedVariableTypeName);
        }
    }
}

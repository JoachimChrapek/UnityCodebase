using FazApp.EditorExtensions.Editor;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace FazApp.SharedVariables.Editor
{
    public class SharedVariablesInspectorWindow : ExtendedEditorWindow
    {
        private const string WindowName = "Shared Variables Inspector";

        private SharedVariableScriptableObjectsContainer scriptableObjectsContainer;
        private SharedVariablesInspectorData inspectorData;
        
        private EditorSharedVariablesInspector editorInspector;
        private RuntimeSharedVariablesInspector runtimeInspector;

        private readonly SearchBarController searchBarController = new();
        private readonly ScrollControlller scrollController = new();
        
        [MenuItem(EditorValues.MenuItemRoot + WindowName)]
        private static void ShowWindow()
        {
            SharedVariablesInspectorWindow windowInstance = GetWindow<SharedVariablesInspectorWindow>(WindowName);
            windowInstance.Initialize();
        }

        protected void OnGUI()
        {
            if (!IsInitialized())
            {
                Initialize();
            }
 
            DrawInspector();
        }

        private void DrawInspector()
        {
            //TODO verify if we need refresh button - it should be automatic
            ExtendedGUI.DrawButton("Refresh", RefreshWindow);

            GUILayout.Space(10);
            
            searchBarController.DrawSearchBar();
            
            GUILayout.Space(10);
            
            DrawScrollableContent();
        }

        private void DrawScrollableContent()
        {
            scrollController.BeginScrollView();
            
            if (Application.isPlaying)
            {
                runtimeInspector.DrawInspector();
            }
            else
            {
                editorInspector.DrawInspector();
            }
            
            scrollController.EndScrollView();
        }

        private bool IsInitialized()
        {
            return scriptableObjectsContainer != null && inspectorData != null && editorInspector != null && runtimeInspector != null;
        }
        
        private void Initialize()
        {
            scriptableObjectsContainer = new SharedVariableScriptableObjectsContainer();
            inspectorData = new SharedVariablesInspectorData();
            
            editorInspector = InitializeInspector<EditorSharedVariablesInspector>();
            runtimeInspector = InitializeInspector<RuntimeSharedVariablesInspector>();
            
            RefreshSharedVariablesTypesCollection();
        }

        private T InitializeInspector<T>() where T : SharedVariablesInspector, new()
        {
            T inspector = new ();
            inspector.Initialize(inspectorData, searchBarController);

            inspector.reinitializeRequested += ReinitializeWindow;
            inspector.refreshRequested += RefreshWindow;

            return inspector;
        }

        private void ReinitializeWindow()
        {
            scriptableObjectsContainer.RefreshScriptableObjectsCollection();
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
            IEnumerable<Type> sharedVariablesTypeCollection = SharedVariablesUtilities.GetSharedVariablesTypeCollection();
            
            foreach (Type sharedVariableType in sharedVariablesTypeCollection)
            {
                SharedVariableScriptableObject scriptableObjectInstance = scriptableObjectsContainer.GetSharedVariableScriptableObject(sharedVariableType);
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
    }
}

using FazApp.EditorExtensions.Editor;
using System;
using UnityEditor;
using UnityEngine;

namespace FazApp.SharedVariables.Unity.Editor
{
    public class EditorSharedVariablesInspector : SharedVariablesInspector
    {
        public override void DrawInspector()
        {
            base.DrawInspector();
            
            DrawSharedVariablesTypeCollection();            
        }
        
        private void DrawSharedVariablesTypeCollection()
        {
            foreach (SharedVariableTypeData sharedVariableTypeData in InspectorData.SharedVariablesTypeDataCollection)
            {
                if(CanDrawSharedVariable(sharedVariableTypeData.SharedVariableType))
                {
                    DrawSharedVariable(sharedVariableTypeData);
                }
            }
        }
        
        private void DrawSharedVariable(SharedVariableTypeData sharedVariableTypeData)
        {
            GUILayout.BeginVertical("box");
            
            ExtendedGUI.DrawLabel(sharedVariableTypeData.SharedVariableType.FullName);
            DrawSharedVariableScriptableObject(sharedVariableTypeData);
            
            GUILayout.EndVertical();
        }
        
        private void DrawSharedVariableScriptableObject(SharedVariableTypeData sharedVariableTypeData)
        {
            GUILayout.BeginHorizontal();
            ExtendedGUI.DrawLabel("Scriptable object:", GUILayout.Width(400));

            if (sharedVariableTypeData.ScriptableObjectInstance == null)
            {
                if (sharedVariableTypeData.HaveScriptableObjectType == false)
                {
                    ExtendedGUI.DrawLabel($"Couldn't find SharedVariableScriptableObject with value type {sharedVariableTypeData.ShredVariableValueType.FullName}");
                }
                else
                {
                    DrawDelayedButton("Create scriptable object", () => CreateScriptableObjectForSharedVariableType(sharedVariableTypeData));
                }
            }
            else
            {
                GUI.enabled = false;
                EditorGUILayout.ObjectField(sharedVariableTypeData.ScriptableObjectInstance, typeof(SharedVariableScriptableObject), false);
                GUI.enabled = true;
            }
            
            GUILayout.EndHorizontal();
        }
        
        private void CreateScriptableObjectForSharedVariableType(SharedVariableTypeData sharedVariableTypeData)
        {
            Type sharedVariableScriptableObjectType = GetSharedVariableScriptableObjectType(sharedVariableTypeData.SharedVariableType);
            string filePath = EditorUtility.SaveFilePanelInProject("Save Shared Variable Scriptable Object", sharedVariableTypeData.SharedVariableType.Name, "asset", "gdzie to jest");
            SharedVariableScriptableObject scriptableObjectInstance = ScriptableObject.CreateInstance(sharedVariableScriptableObjectType) as SharedVariableScriptableObject;
            scriptableObjectInstance.SetAssignedSharedVariableTypeName(sharedVariableTypeData.SharedVariableType);
            
            AssetDatabase.CreateAsset(scriptableObjectInstance, filePath);
            
            SharedVariableScriptableObject scriptableObjectAsset = AssetDatabase.LoadAssetAtPath<SharedVariableScriptableObject>(filePath);
            // ScriptableObjectsContainer.Add(scriptableObjectAsset);

            EditorUtility.SetDirty(scriptableObjectAsset);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
            ReinitializeInspectorWindow();
        }
        
        private Type GetSharedVariableScriptableObjectType(Type sharedVariableType)
        {
            Type valueType = SharedVariablesUtilities.GetSharedVariableValueType(sharedVariableType);
            return InspectorData.ValueTypeToSharedVariableScriptableObjectTypeMap[valueType];
        }
    }
}

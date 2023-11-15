using FazApp.EditorExtensions.Editor;
using FazApp.Logging;
using UnityEditor;
using UnityEngine;

namespace FazApp.SharedVariables.Editor
{
    public class RuntimeSharedVariablesInspector : SharedVariablesInspector
    {
        private RuntimeSharedVariableInspectorDataContainer runtimeSharedVariablesContainer;
        private SerializedObject sharedVariablesContainerSerializedObject;
        private SerializedProperty sharedVariablesCollectionProperty;

        private UnityEditor.Editor cachedSharedVariabledContainerEditor;
        
        ~RuntimeSharedVariablesInspector()
        {
            runtimeSharedVariablesContainer.DecomissionSharedVariables();
        }

        protected override void DrawInspectorContent()
        {
            if (!IsInitialized())
            {
                Initialize();
            }

            sharedVariablesContainerSerializedObject.Update();
            DrawSharedVariablesCollection();
        }
        
        private void DrawSharedVariablesCollection()
        {
            for (int i = 0; i < runtimeSharedVariablesContainer.SharedVariablesCollection.Count; i++)
            {
                RuntimeSharedVariableInspectorData sharedVariableData = runtimeSharedVariablesContainer.SharedVariablesCollection[i];
                SharedVariable sharedVariable = sharedVariableData.CachedSharedVariable;
                SerializedProperty sharedVariableSerializedProperty = sharedVariablesCollectionProperty.GetArrayElementAtIndex(i);

                DrawSharedVariable(sharedVariable, sharedVariableData, sharedVariableSerializedProperty);
                GUILayout.Space(5.0f);
            }
        }

        private void DrawSharedVariable(SharedVariable sharedVariable, RuntimeSharedVariableInspectorData sharedVariableData, SerializedProperty sharedVariableSerializedProperty)
        {
            if (!CanDrawSharedVariable(sharedVariable.GetType()))
            {
                return;
            }
            
            GUILayout.BeginVertical("box");

            ExtendedGUI.DrawLabel(sharedVariable.GetType().FullName);

            DrawSharedVariableEditorValue(sharedVariableSerializedProperty, out bool valueChanged);
            HandleEditorValueChange(valueChanged);
            
            GUILayout.Space(5.0f);
            
            sharedVariableData.IsAutoSaveEnabled = EditorGUILayout.Toggle("Auto save", sharedVariableData.IsAutoSaveEnabled);
            HandleAutoSaveOptions(sharedVariable, sharedVariableData.IsAutoSaveEnabled, valueChanged);

            ExtendedGUI.DrawButton("Notify value changed", sharedVariable.ForceNotifyValueChanged);
            
            GUILayout.EndVertical();
        }

        private void DrawSharedVariableEditorValue(SerializedProperty sharedVariableSerializedProperty, out bool valueChanged)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty property = sharedVariableSerializedProperty.FindPropertyRelative(EditorUtils.GetBackingFieldName(nameof(RuntimeSharedVariableInspectorData.CachedSharedVariable)) + "." + EditorUtils.GetBackingFieldName("EditorValue"));
            EditorGUILayout.PropertyField(property, new GUIContent("Value:"), true);
            valueChanged = EditorGUI.EndChangeCheck();
        }

        private void HandleEditorValueChange(bool valueChanged)
        {
            if (!valueChanged)
            {
                return;
            }

            sharedVariablesContainerSerializedObject.ApplyModifiedProperties();
            sharedVariablesContainerSerializedObject.Update();
        }

        private void HandleAutoSaveOptions(SharedVariable sharedVariable, bool isAutoSaveEnabled, bool valueChanged)
        {
            if (isAutoSaveEnabled)
            {
                if (valueChanged)
                {
                    sharedVariable.UpdateValueAfterEditorChange();
                }
            }

            GUI.enabled = !isAutoSaveEnabled;
            EditorGUILayout.BeginHorizontal();
            
            ExtendedGUI.DrawButton("Save value", sharedVariable.UpdateValueAfterEditorChange);
            ExtendedGUI.DrawButton("Reset value", sharedVariable.UpdateEditorValue);
                
            EditorGUILayout.EndHorizontal();
            GUI.enabled = true;
        }
        
        private bool IsInitialized()
        {
            return runtimeSharedVariablesContainer != null && runtimeSharedVariablesContainer.SharedVariablesCollection != null && sharedVariablesContainerSerializedObject != null && sharedVariablesCollectionProperty != null;
        }

        private void Initialize()
        {
            InitializeSharedVariablesContainer();
            
            sharedVariablesContainerSerializedObject = new SerializedObject(runtimeSharedVariablesContainer);
            sharedVariablesCollectionProperty = sharedVariablesContainerSerializedObject.FindProperty(EditorUtils.GetBackingFieldName(nameof(RuntimeSharedVariableInspectorDataContainer.SharedVariablesCollection)));
        }
        
        private void InitializeSharedVariablesContainer()
        {
            runtimeSharedVariablesContainer = ScriptableObject.CreateInstance<RuntimeSharedVariableInspectorDataContainer>();

            foreach (SharedVariableTypeData sharedVariableTypeData in InspectorData.SharedVariablesTypeDataCollection)
            {
                if (!SV.TryGet(sharedVariableTypeData.SharedVariableType, out ISharedVariable sharedVariableInterface))
                {
                    Log.Warning($"Couldn't get shared variable for type: {sharedVariableTypeData.SharedVariableType}");
                    continue;
                }

                RuntimeSharedVariableInspectorData data = new(sharedVariableInterface as SharedVariable);
                runtimeSharedVariablesContainer.SharedVariablesCollection.Add(data);
            }

            runtimeSharedVariablesContainer.InitializeSharedVariables();
        }
    }
}
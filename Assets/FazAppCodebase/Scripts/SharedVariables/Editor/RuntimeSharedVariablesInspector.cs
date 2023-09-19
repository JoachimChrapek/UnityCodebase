using FazApp.EditorExtensions.Editor;
using UnityEditor;
using UnityEngine;

namespace FazApp.SharedVariables.Editor
{
    public class RuntimeSharedVariablesInspector : SharedVariablesInspector
    {
        private SharedVariableContainer sharedVariablesContainer;
        private SerializedObject sharedVariablesContainerSerializedObject;
        private SerializedProperty sharedVariablesCollectionProperty;

        private UnityEditor.Editor cachedSharedVariabledContainerEditor;
        
        ~RuntimeSharedVariablesInspector()
        {
            sharedVariablesContainer.DecomissionSharedVariables();
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
            for (int i = 0; i < sharedVariablesContainer.SharedVariablesCollection.Count; i++)
            {
                RuntimeSharedVariableInspectorData sharedVariableData = sharedVariablesContainer.SharedVariablesCollection[i];
                SharedVariable sharedVariable = sharedVariableData.CachedSharedVariable;
                SerializedProperty sharedVariableSerializedProperty = sharedVariablesCollectionProperty.GetArrayElementAtIndex(i);

                DrawSharedVariable(sharedVariable, sharedVariableData, sharedVariableSerializedProperty);
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
            sharedVariableData.IsAutoSaveEnabled = EditorGUILayout.Toggle("Auto save", sharedVariableData.IsAutoSaveEnabled);

            DrawSharedVariableEditorValue(sharedVariableSerializedProperty, out bool valueChanged);
            HandleEditorValueChange(valueChanged);
            HandleAutoSaveOptions(sharedVariable, sharedVariableData.IsAutoSaveEnabled, valueChanged);

            ExtendedGUI.DrawButton("Notify value changed", sharedVariable.ForceNotifyValueChanged);
            
            GUILayout.EndVertical();
        }

        private void DrawSharedVariableEditorValue(SerializedProperty sharedVariableSerializedProperty, out bool valueChanged)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(sharedVariableSerializedProperty.FindPropertyRelative(EditorUtils.GetBackingFieldName(nameof(RuntimeSharedVariableInspectorData.CachedSharedVariable)) + "." + EditorUtils.GetBackingFieldName("EditorValue")), true);
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
            else
            {
                EditorGUILayout.BeginHorizontal();
                
                ExtendedGUI.DrawButton("Save value", sharedVariable.UpdateValueAfterEditorChange);
                ExtendedGUI.DrawButton("Reset value", sharedVariable.UpdateEditorValue);
                
                EditorGUILayout.EndHorizontal();
            }
        }
        
        private bool IsInitialized()
        {
            return sharedVariablesContainer != null && sharedVariablesContainer.SharedVariablesCollection != null && sharedVariablesContainerSerializedObject != null && sharedVariablesCollectionProperty != null;
        }

        private void Initialize()
        {
            InitializeSharedVariablesContainer();
            
            sharedVariablesContainerSerializedObject = new SerializedObject(sharedVariablesContainer);
            sharedVariablesCollectionProperty = sharedVariablesContainerSerializedObject.FindProperty(EditorUtils.GetBackingFieldName(nameof(SharedVariableContainer.SharedVariablesCollection)));
        }
        
        private void InitializeSharedVariablesContainer()
        {
            sharedVariablesContainer = ScriptableObject.CreateInstance<SharedVariableContainer>();

            foreach (SharedVariableTypeData sharedVariableTypeData in InspectorData.SharedVariablesTypeDataCollection)
            {
                if (!SV.TryGet(sharedVariableTypeData.SharedVariableType, out ISharedVariable sharedVariableInterface))
                {
                    //TODO log
                    continue;
                }

                if (sharedVariableInterface is not SharedVariable sharedVariable)
                {
                    //TODO log
                    continue;
                }

                RuntimeSharedVariableInspectorData data = new(sharedVariable);

                sharedVariablesContainer.SharedVariablesCollection.Add(data);
            }

            sharedVariablesContainer.InitializeSharedVariables();
        }
    }
}
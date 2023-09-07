using FazApp.Core.Unity.Editor;
using UnityEditor;
using UnityEngine;

namespace FazApp.SharedVariables.Unity.Editor
{
    public class RuntimeSharedVariablesInspector : SharedVariablesInspector
    {
        private SharedVariableContainer sharedVaraiblesContainer;
        private SerializedObject sharedVaraiblesContainerSerializedObject;
        private SerializedProperty sharedVariablesCollectionProperty;

        private UnityEditor.Editor cachedSharedVariabledContainerEditor;

        ~RuntimeSharedVariablesInspector()
        {
            foreach (RuntimeSharedVariableInspectorData sharedVariableData in sharedVaraiblesContainer.SharedVariablesCollection)
            {
                sharedVariableData.CachedSharedVariable.editorValueChanged -= RefreshInspectorWindow;
            }

            sharedVaraiblesContainer.DecomissionSharedVariables();
        }

        public override void DrawInspector()
        {
            if (!IsSharedVariablesContainerInitialized())
            {
                InitializeSharedVariablesContainer();
            }

            base.DrawInspector();

            if (sharedVaraiblesContainerSerializedObject == null)
            {
                sharedVaraiblesContainerSerializedObject = new SerializedObject(sharedVaraiblesContainer);
            }

            if (sharedVariablesCollectionProperty == null)
            {
                sharedVariablesCollectionProperty = sharedVaraiblesContainerSerializedObject.FindProperty(EditorUtils.GetBackingFieldName(nameof(SharedVariableContainer.SharedVariablesCollection)));
            }
            
            sharedVaraiblesContainerSerializedObject.Update();

            for (int i = 0; i < sharedVaraiblesContainer.SharedVariablesCollection.Count; i++)
            {
                RuntimeSharedVariableInspectorData sharedVariableData = sharedVaraiblesContainer.SharedVariablesCollection[i];
                SharedVariable sharedVariable = sharedVariableData.CachedSharedVariable;

                if (!CanDrawSharedVariable(sharedVariable.GetType()))
                {
                    break;
                }
                
                GUILayout.BeginVertical("box");

                ExtendedGUI.DrawLabel(sharedVariable.GetType().FullName);

                sharedVariableData.IsAutoSaveEnabled = EditorGUILayout.Toggle("Auto save", sharedVariableData.IsAutoSaveEnabled);

                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(sharedVariablesCollectionProperty.GetArrayElementAtIndex(i).FindPropertyRelative(EditorUtils.GetBackingFieldName(nameof(RuntimeSharedVariableInspectorData.CachedSharedVariable)) + "." + EditorUtils.GetBackingFieldName("EditorValue")), true);
                
                bool valueChanged = EditorGUI.EndChangeCheck();

                if (valueChanged)
                {
                    sharedVaraiblesContainerSerializedObject.ApplyModifiedProperties();
                    sharedVaraiblesContainerSerializedObject.Update();
                }
                
                if (sharedVariableData.IsAutoSaveEnabled)
                {
                    if (valueChanged)
                    {
                        sharedVariable.UpdateValueAfterEditorChange();
                    }
                }
                else
                {
                    if (GUILayout.Button("Save value"))
                    {
                        sharedVariable.UpdateValueAfterEditorChange();
                    }

                    if (GUILayout.Button("Reset value"))
                    {
                        sharedVariable.UpdateEditorValue();
                    }
                }

                if (GUILayout.Button("Notify value changed"))
                {
                    sharedVariable.ForceNotifyValueChanged();
                }

                GUILayout.EndVertical();
            }
        }

        private bool IsSharedVariablesContainerInitialized()
        {
            return sharedVaraiblesContainer != null && sharedVaraiblesContainer.SharedVariablesCollection != null;
        }

        private void InitializeSharedVariablesContainer()
        {
            sharedVaraiblesContainer = ScriptableObject.CreateInstance<SharedVariableContainer>();

            foreach (SharedVariableTypeData sharedVariableTypeData in InspectorData.SharedVariablesTypeDataCollection)
            {
                if (SV.TryGet(sharedVariableTypeData.SharedVariableType, out ISharedVariable sharedVariableInterface))
                {
                    if (sharedVariableInterface is not SharedVariable sharedVariable)
                    {
                        //TODO log
                        continue;
                    }

                    sharedVariable.editorValueChanged += RefreshInspectorWindow;
                    RuntimeSharedVariableInspectorData data = new(sharedVariable);

                    sharedVaraiblesContainer.SharedVariablesCollection.Add(data);
                }
            }

            sharedVaraiblesContainer.InitializeSharedVariables();
        }
    }
}
using UnityEditor;
using UnityEngine;

namespace FazApp.Core.Editor
{
    [CustomPropertyDrawer(typeof(MinValueAttribute))]
    public class MinValuePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.PropertyField(position, property, label);

            if (!EditorGUI.EndChangeCheck())
            {
                return;
            }

            double minValue = ((MinValueAttribute)attribute).MinValue;

            
        }
    }
}

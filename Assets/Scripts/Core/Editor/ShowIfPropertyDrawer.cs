// using UnityEditor;
// using UnityEngine;
//
// namespace FazApp.Core.Editor
// {
//     [CustomPropertyDrawer(typeof(ShowIfAttribute))]
//     public class ShowIfPropertyDrawer : PropertyDrawer
//     {
//         public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//         {
//             ShowIfAttribute showIfAttribute = attribute as ShowIfAttribute;
//             
//             if (showIfAttribute.condition.Invoke())
//             {
//                 EditorGUI.PropertyField(position, property, label);
//             }
//             else
//             {
//                 EditorGUI.LabelField(position, "Dupka");
//             }
//         }
//     }
// }

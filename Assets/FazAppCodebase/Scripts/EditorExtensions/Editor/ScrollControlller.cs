using UnityEditor;
using UnityEngine;

namespace FazApp.EditorExtensions.Editor
{
    public class ScrollControlller
    {
        private Vector2 scrollPosition;
        
        public void BeginScrollView()
        {
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        }

        public void EndScrollView()
        {
            EditorGUILayout.EndScrollView();
        }
    }
}

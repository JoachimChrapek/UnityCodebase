using System;
using UnityEngine;

namespace FazApp.Core.Unity.Editor
{
    public static class ExtendedGUI
    {
        public static void DrawLabel(string labelText, params GUILayoutOption[] options)
        {
            GUILayout.Label(labelText, options);
            
        }

        public static void DrawButton(string buttonText, Action onButtonClicked)
        {
            if (GUILayout.Button(buttonText))
            {
                onButtonClicked?.Invoke();
            }
        }
    }
}

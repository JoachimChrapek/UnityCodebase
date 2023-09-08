using UnityEditor;

namespace FazApp.EditorExtensions.Editor
{
    public class SearchBarController
    {
        public string SearchText { get; private set; }

        public void DrawSearchBar()
        {
            SearchText = EditorGUILayout.TextField("Search", SearchText);
        }
    }
}

namespace FazApp.Core.Unity.Editor
{
    public static class EditorUtils
    {
        public static string GetBackingFieldName(string propertyName)
        {
            return $"<{propertyName}>k__BackingField";
        }
    }
}

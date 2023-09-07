namespace FazApp.EditorExtensions.Editor
{
    public static class EditorUtils
    {
        public static string GetBackingFieldName(string propertyName)
        {
            return $"<{propertyName}>k__BackingField";
        }
    }
}

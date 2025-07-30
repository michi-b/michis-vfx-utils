using UnityEditor;

namespace Michis.VfxUtils.Editor.Extensions
{
    public static class SerializedPropertyExtensions
    {
        public static SerializedProperty FindRequiredPropertyRelative(this SerializedProperty property, string name)
        {
            SerializedProperty result = property.FindPropertyRelative(name);
            if (result == null)
            {
                throw new System.Exception($"Property '{name}' not found in '{property.propertyPath}'");
            }
            return result;
        }
    }
}
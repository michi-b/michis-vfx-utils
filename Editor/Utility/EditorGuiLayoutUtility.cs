using UnityEditor;
using UnityEngine;

namespace MichisVfxUtils.Editor.Utility
{
    public static class EditorGuiLayoutUtility
    {
        public static void ToggleLeftProperty(SerializedProperty property, GUIContent label)
        {
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            EditorGUI.BeginChangeCheck();
            var value = EditorGUILayout.ToggleLeft(label, property.boolValue);
            if (EditorGUI.EndChangeCheck()) property.boolValue = value;
        }
    }
}
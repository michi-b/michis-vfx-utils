using System.IO;
using UnityEditor;
using UnityEngine;

namespace MichisUnityVfxUtilities.MichisUnityVfxUtilities.Editor.Utility
{
    public static class SelectionUtility
    {
        public static string GetActiveFolder()
        {
            var activeObject = Selection.activeObject;
            if (activeObject != null)
            {
                var selectedPath = AssetDatabase.GetAssetPath(activeObject);
                return AssetDatabase.IsValidFolder(selectedPath)
                    ? selectedPath
                    : Path.GetDirectoryName(selectedPath);
            }

            foreach (var selectedObject in Selection.GetFiltered<Object>(SelectionMode.Assets))
            {
                var selectedPath = AssetDatabase.GetAssetPath(selectedObject);
                if (!string.IsNullOrEmpty(selectedPath) && File.Exists(selectedPath))
                    return Path.GetDirectoryName(selectedPath);
            }

            return "Assets";
        }
    }
}
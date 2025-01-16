using System.IO;
using UnityEditor;
using UnityEngine;

namespace Michis.VfxUtils.Editor.Utility
{
    public static class SelectionUtility
    {
        public static string GetActiveFolder()
        {
            Object activeObject = Selection.activeObject;
            if (activeObject != null)
            {
                string selectedPath = AssetDatabase.GetAssetPath(activeObject);
                return AssetDatabase.IsValidFolder(selectedPath)
                    ? selectedPath
                    : Path.GetDirectoryName(selectedPath);
            }

            foreach (Object selectedObject in Selection.GetFiltered<Object>(SelectionMode.Assets))
            {
                string selectedPath = AssetDatabase.GetAssetPath(selectedObject);
                if (!string.IsNullOrEmpty(selectedPath) && File.Exists(selectedPath))
                {
                    return Path.GetDirectoryName(selectedPath);
                }
            }

            return "Assets";
        }
    }
}
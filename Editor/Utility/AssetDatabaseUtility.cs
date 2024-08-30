using System.IO;
using UnityEditor;
using UnityEngine;

namespace MichisMeshMakers.Editor.Utility
{
    public static class AssetDatabaseUtility
    {
        public static string GetUniqueAssetPathInActiveFolder(string assetName)
        {
            string activeFolder = SelectionUtility.GetActiveFolder();
            return AssetDatabase.GenerateUniqueAssetPath(Path.Combine(activeFolder, $"{assetName}.asset"));
        }

        public static void ForceSaveAsset(Object asset, bool instantRefresh = false)
        {
            EditorUtility.SetDirty(asset);
            AssetDatabase.SaveAssetIfDirty(asset);
            if (instantRefresh)
            {
                AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate | ImportAssetOptions.ForceSynchronousImport);
            }
        }
    }
}
using System.IO;
using MichisVfxUtils.Editor.Containers.Abstract;
using UnityEditor;
using UnityEngine;

namespace MichisVfxUtils.Editor.Utility
{
    public static class AssetDatabaseUtility
    {
        public static string GetUniqueAssetPathInActiveFolder(string assetName)
        {
            var activeFolder = SelectionUtility.GetActiveFolder();
            return AssetDatabase.GenerateUniqueAssetPath(Path.Combine(activeFolder, $"{assetName}.asset"));
        }

        public static void ForceSaveAsset(Object asset, bool instantRefresh = false)
        {
            EditorUtility.SetDirty(asset);
            AssetDatabase.SaveAssetIfDirty(asset);
            if (instantRefresh)
                AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate | ImportAssetOptions.ForceSynchronousImport);
        }

        public static bool GetIsInWritableFolder(Object asset)
        {
            return GetIsInWritableFolder(AssetDatabase.GetAssetPath(asset));
        }

        public static bool GetIsInWritableFolder(string assetPath)
        {
            // strip file name from path
            var folderPath = Path.GetDirectoryName(assetPath);
            return AssetDatabase.IsOpenForEdit(folderPath);
        }

        public static T LoadSelectedObject<T>() where T : Object
        {
            return AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GetAssetPath(Selection.activeObject));
        }

        /// <summary>
        /// Loads an object of type <typeparamref name="T"/> from the same asset as the <paramref name="meshContainer"/>.
        /// </summary>
        public static T LoadFromSameAsset<T>(MeshContainer meshContainer) where T : Object
        {
            return AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GetAssetPath(meshContainer));
        }
    }
}
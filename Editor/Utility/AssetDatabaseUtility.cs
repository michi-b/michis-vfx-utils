using System.IO;
using MichisMeshMakers.Editor.Containers;
using MichisMeshMakers.Editor.Containers.Abstract;
using UnityEditor;

namespace MichisMeshMakers.Editor.Utility
{
    public static class AssetDatabaseUtility
    {
        public static void ForceInstantRefresh()
        {
            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate | ImportAssetOptions.ForceSynchronousImport);
        }

        public static string GetCreateAssetPath(string assetName)
        {
            string activeFolder = SelectionUtility.GetActiveFolder();
            return AssetDatabase.GenerateUniqueAssetPath(Path.Combine(activeFolder, $"{assetName}.asset"));
        }

        public static void ForceSaveAsset(MeshContainer meshContainer, bool instantRefresh = false)
        {
            EditorUtility.SetDirty(meshContainer);
            AssetDatabase.SaveAssetIfDirty(meshContainer);
            if (instantRefresh)
            {
                ForceInstantRefresh();
            }
        }
    }
}
using System.IO;
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
            // Create Parent Asset
            return AssetDatabase.GenerateUniqueAssetPath(Path.Combine(SelectionUtility.GetActiveFolder(), $"{assetName}.asset"));
        }

        public static string GetDependentFilename(string originName, string individualName)
        {
            return originName == null ? individualName : $"{originName}_{individualName}";
        }
    }
}
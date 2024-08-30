using UnityEditor;

namespace MichisMeshMakers.Editor
{
    public static class EditorPreferences
    {
        private const string KeyPrefix = "MichisMeshMakers.";
        private const string MeshAssetNamePostFixKey = KeyPrefix + "DefaultMeshAssetNamePostFix";
        private const string MeshContainerAssetNamePostFixKey = KeyPrefix + "DefaultMeshContainerAssetNamePostFix";

        private static string _meshAssetNamePostFix = "_Mesh";
        private static string _meshContainerAssetNamePostFix = "_MeshContainer";

        static EditorPreferences()
        {
            _meshAssetNamePostFix = EditorPrefs.GetString(MeshAssetNamePostFixKey, _meshAssetNamePostFix);
            _meshContainerAssetNamePostFix = EditorPrefs.GetString(MeshContainerAssetNamePostFixKey, _meshContainerAssetNamePostFix);
        }

        public static string MeshAssetNamePostFix
        {
            get => _meshAssetNamePostFix;
            set
            {
                _meshAssetNamePostFix = value;
                EditorPrefs.SetString(MeshAssetNamePostFixKey, value);
            }
        }

        public static string MeshContainerAssetNamePostFix
        {
            get => _meshContainerAssetNamePostFix;
            set
            {
                _meshContainerAssetNamePostFix = value;
                EditorPrefs.SetString(MeshContainerAssetNamePostFixKey, value);
            }
        }
    }
}
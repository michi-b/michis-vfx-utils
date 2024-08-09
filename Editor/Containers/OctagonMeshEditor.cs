using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MichisMeshMakers.Editor.Containers
{
    [CustomEditor(typeof(OctagonMesh))]
    public class OctagonMeshEditor : MeshContainerEditor<OctagonMesh>
    {
        private const string AssetMenuName = "Octagon Mesh";
        private const string AssetFileName = "OctagonMesh";

        [MenuItem(Menu.CreateAssetPath + AssetMenuName)]
        public static void CreateParentAssetWithMesh()
        {
            Create(AssetFileName, GetCreationTarget);
        }
        
        private static Object GetCreationTarget(string selectionPath)
        {
            return AssetDatabase.LoadAssetAtPath<Texture2D>(selectionPath);
        }
    }
}
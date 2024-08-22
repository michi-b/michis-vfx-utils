using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MichisMeshMakers.Editor.Containers
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(OctagonMesh))]
    public class OctagonMeshEditor : TextureBasedMeshContainerEditor<OctagonMesh>
    {
        private const string AssetMenuName = "Octagon Mesh";
        private const string AssetFileName = "OctagonMesh";
        
        private SerializedProperty _axisLengthProperty;
        private SerializedProperty _diagonalLengthProperty;
        
        protected override void OnEnable()
        {
            base.OnEnable();
            _axisLengthProperty = serializedObject.FindProperty(OctagonMesh.AxisLengthFieldName);
            _diagonalLengthProperty = serializedObject.FindProperty(OctagonMesh.DiagonalLengthFieldName);
        }

        protected override void DrawProperties()
        {
            base.DrawProperties();
            
            EditorGUILayout.PropertyField(_axisLengthProperty);
            EditorGUILayout.PropertyField(_diagonalLengthProperty);
        }

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
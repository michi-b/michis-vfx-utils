using MichisMeshMakers.Editor.Containers.Abstract;
using MichisMeshMakers.Editor.Containers.Abstract.Generic;
using MichisMeshMakers.Editor.Utility;
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
        private const string FallbackAssetName = "OctagonMesh";

        private const string ConstrainLengthsTooltip = "Whether the lengths are set with capped sliders to remain within a unit quad.";
        private static readonly GUIContent ConstrainLengthsLabel = new GUIContent("Display Lengths As Sliders", ConstrainLengthsTooltip);
        private static readonly GUIContent AxisLengthLabel = new GUIContent("Axis Length", "The size of the octagon along the x and y axis.");
        private static readonly GUIContent DiagonalLengthLabel = new GUIContent("Diagonal Length", "The size of the octagon along the diagonals.");
        private SerializedProperty _axisLengthProperty;

        private SerializedProperty _constrainLengthsProperty;
        private SerializedProperty _diagonalLengthProperty;

        protected override void OnEnable()
        {
            base.OnEnable();
            _constrainLengthsProperty = serializedObject.FindProperty(OctagonMesh.ConstrainLengthsFieldName);
            _axisLengthProperty = serializedObject.FindProperty(OctagonMesh.AxisLengthFieldName);
            _diagonalLengthProperty = serializedObject.FindProperty(OctagonMesh.DiagonalLengthFieldName);
        }

        protected override void DrawProperties()
        {
            base.DrawProperties();

            EditorGUI.BeginChangeCheck();
            EditorGuiLayoutUtility.ToggleLeftProperty(_constrainLengthsProperty, ConstrainLengthsLabel);
            bool lengthsAreUnconstrained = _constrainLengthsProperty.hasMultipleDifferentValues || !_constrainLengthsProperty.boolValue;
            if (lengthsAreUnconstrained)
            {
                DrawUnconstrainedLengthProperty(_axisLengthProperty, AxisLengthLabel);
                DrawUnconstrainedLengthProperty(_diagonalLengthProperty, DiagonalLengthLabel);
            }
            else
            {
                EditorGUILayout.PropertyField(_axisLengthProperty, AxisLengthLabel);
                EditorGUILayout.PropertyField(_diagonalLengthProperty, DiagonalLengthLabel);
            }

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                Apply();
            }
        }

        private static void DrawUnconstrainedLengthProperty(SerializedProperty property, GUIContent label)
        {
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            EditorGUI.BeginChangeCheck();
            float value = EditorGUILayout.FloatField(property.displayName, property.floatValue);
            if (EditorGUI.EndChangeCheck())
            {
                value = Mathf.Max(value, 0f); // still constrain the value to be >= 0f
                property.floatValue = value;
            }
        }

        [MenuItem(Menu.CreateAssetPath + AssetMenuName)]
        public static void Create()
        {
            var selectedTexture = AssetDatabaseUtility.LoadSelectedObject<Texture2D>();
            MeshContainer _ = selectedTexture != null && AssetDatabaseUtility.GetIsInWritableFolder(selectedTexture)
                ? Create(selectedTexture)
                : Create(AssetDatabaseUtility.GetUniqueAssetPathInActiveFolder(FallbackAssetName), null);
        }

        public static OctagonMesh Create(Texture2D textureAsset)
        {
            return Create(GetCreateAssetPathFromTarget(textureAsset), meshContainer => { Initialize(meshContainer, textureAsset); });
        }
    }
}
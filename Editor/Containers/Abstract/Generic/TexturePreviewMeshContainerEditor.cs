using MichisVfxUtils.Editor.Assets;
using MichisVfxUtils.Editor.Utility;
using UnityEditor;
using UnityEngine;

namespace MichisVfxUtils.Editor.Containers.Abstract.Generic
{
    public abstract class TexturePreviewMeshContainerEditor<TMeshContainer>
        : MeshContainerEditor<TMeshContainer> where TMeshContainer : TexturePreviewMeshContainer
    {
        private SerializedProperty _textureProperty;
        private SerializedProperty _previewMaterialProperty;
        private SerializedProperty _previewSettingsProperty;

        protected override void OnEnable()
        {
            base.OnEnable();
            _previewSettingsProperty = serializedObject.FindProperty(TexturePreviewMeshContainer.PreviewSettingsFieldName);
            _textureProperty = _previewSettingsProperty.FindPropertyRelative(TexturePreviewSettings.TextureFieldName);
            _previewMaterialProperty = _previewSettingsProperty.FindPropertyRelative(TexturePreviewSettings.TexturePreviewMaterialFieldName);
        }

        protected override void DrawProperties()
        {
            base.DrawProperties();
            DrawTexturePreviewSettings();
        }

        protected virtual void DrawTexturePreviewSettings()
        {
            _previewSettingsProperty.isExpanded = EditorGUILayout.BeginFoldoutHeaderGroup(_previewSettingsProperty.isExpanded, PreviewSettingsHeader);

            if (_previewSettingsProperty.isExpanded)
            {
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(_textureProperty, MeshContainerGuiLabels.SourceTexture);
                EditorGUILayout.PropertyField(_previewMaterialProperty);
                if (EditorGUI.EndChangeCheck())
                {
                    serializedObject.ApplyModifiedProperties();
                }
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
        }

        protected override void DrawMeshPreview(Rect rect, TMeshContainer meshContainer)
        {
            DrawCanvas(rect);
            Texture2D texture = meshContainer.PreviewSettings.Texture;
            if (texture != null)
            {
                Material previewMaterial = EditorAssets.MaterialInstances.GetTexturePreviewMaterial(meshContainer.PreviewSettings.Material, texture);
                EditorGUI.DrawPreviewTexture(rect, texture, previewMaterial, ScaleMode.StretchToFill);
            }

            DrawMesh(rect, meshContainer.Mesh);
        }

        protected virtual void DrawCanvas(Rect rect)
        {
            if (Event.current.type == EventType.Layout)
            {
                return;
            }

            Material canvasGridMaterial = EditorAssets.MaterialInstances.GetCanvasGrid(rect);
            EditorGUI.DrawPreviewTexture(rect, EditorAssets.Textures.CanvasGrid, canvasGridMaterial, ScaleMode.ScaleAndCrop);
        }

        protected virtual void DrawMesh(Rect position, Mesh mesh)
        {
            if (Event.current.type == EventType.Layout)
            {
                return;
            }

            Handles.BeginGUI();

            Handles.color = Color.white;
            int triangleCount = mesh.triangles.Length / 3;
            for (int i = 0; i < triangleCount; i++)
            {
                int startIndex = i * 3;
                Buffers.Temporary.TriangleVectors3D[0] = ConvertPoint(mesh.vertices[mesh.triangles[startIndex]]);
                Buffers.Temporary.TriangleVectors3D[1] = ConvertPoint(mesh.vertices[mesh.triangles[startIndex + 1]]);
                Buffers.Temporary.TriangleVectors3D[2] = ConvertPoint(mesh.vertices[mesh.triangles[startIndex + 2]]);
                Handles.DrawLines(Buffers.Temporary.TriangleVectors3D, Buffers.TriangleLineIndices);
            }

            Handles.EndGUI();

            SceneView.RepaintAll();

            return;

            Vector3 ConvertPoint(Vector3 point)
            {
                var xy = new Vector2(point.x, point.y);
                xy.Scale(new Vector2(position.width, position.height));
                Vector2 center = position.center;
                xy += new Vector2(center.x, center.y);
                return new Vector3(xy.x, xy.y, 0f);
            }
        }

        protected static string GetCreateAssetPathFromSelection(string fallbackName)
        {
            var selectedTexture = AssetDatabaseUtility.LoadSelectedObject<Texture2D>();
            return selectedTexture != null ? GetCreateAssetPathFromTarget(selectedTexture) : fallbackName;
        }

        protected static void Initialize(TMeshContainer meshContainer, Texture2D texture)
        {
            var so = new SerializedObject(meshContainer);
            SerializedProperty sp = so.FindProperty(TexturePreviewMeshContainer.PreviewSettingsFieldName)
                .FindPropertyRelative(TexturePreviewSettings.TextureFieldName);
            sp.objectReferenceValue = texture;
            so.ApplyModifiedProperties();
        }

        private static readonly GUIContent PreviewSettingsHeader = new GUIContent("Preview Settings", "Settings related to the texture preview below.");
    }
}
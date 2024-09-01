using MichisVfxUtils.Editor.Assets;
using MichisVfxUtils.Editor.Utility;
using UnityEditor;
using UnityEngine;

namespace MichisVfxUtils.Editor.Containers.Abstract.Generic
{
    public abstract class TextureBasedMeshContainerEditor<TMeshContainer>
        : MeshContainerEditor<TMeshContainer> where TMeshContainer : TextureBasedMeshContainer
    {
        private SerializedProperty _textureProperty;

        protected override void OnEnable()
        {
            base.OnEnable();
            _textureProperty = serializedObject.FindProperty(TextureBasedMeshContainer.TextureFieldName);
        }

        protected override void DrawProperties()
        {
            base.DrawProperties();

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(_textureProperty, MeshContainerGuiLabels.SourceTexture);
            if (EditorGUI.EndChangeCheck()) serializedObject.ApplyModifiedProperties();
        }

        protected override void DrawMeshPreview(Rect rect, TMeshContainer meshContainer)
        {
            DrawCanvas(rect);
            if (meshContainer.Texture != null) DrawTexture(rect, meshContainer.Texture);

            DrawMesh(rect, meshContainer.Mesh);
        }

        protected virtual void DrawCanvas(Rect rect)
        {
            if (Event.current.type == EventType.Layout) return;

            var canvasGridMaterial = EditorAssets.MaterialInstances.CanvasGrid;
            canvasGridMaterial.SetVector(Ids.MaterialProperties.RectSize, new Vector2(rect.width, rect.height));
            EditorGUI.DrawPreviewTexture(rect, EditorAssets.Textures.CanvasGrid, canvasGridMaterial,
                ScaleMode.ScaleAndCrop);
        }

        protected virtual void DrawTexture(Rect position, Texture2D texture)
        {
            var material = EditorAssets.MaterialInstances.Additive;
            material.mainTexture = texture;
            EditorGUI.DrawPreviewTexture(position, texture, material, ScaleMode.StretchToFill);
        }

        protected virtual void DrawMesh(Rect position, Mesh mesh)
        {
            if (Event.current.type == EventType.Layout) return;

            Handles.BeginGUI();

            Handles.color = Color.white;
            var triangleCount = mesh.triangles.Length / 3;
            for (var i = 0; i < triangleCount; i++)
            {
                var startIndex = i * 3;
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
                var center = position.center;
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
            var sp = so.FindProperty(TextureBasedMeshContainer.TextureFieldName);
            sp.objectReferenceValue = texture;
            so.ApplyModifiedProperties();
        }
    }
}
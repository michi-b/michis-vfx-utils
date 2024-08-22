﻿using MichisMeshMakers.Editor.Assets;
using MichisMeshMakers.Editor.Utility;
using UnityEditor;
using UnityEngine;

namespace MichisMeshMakers.Editor.Containers
{
    public abstract class TextureBasedMeshContainerEditor<TMeshContainer>
        : MeshContainerEditor<TMeshContainer>
        where TMeshContainer : TextureBasedMeshContainer
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
            EditorGUILayout.PropertyField(_textureProperty, MeshContainerLabels.SourceTexture);
        }

        protected override void DrawMeshPreview(Rect rect, TMeshContainer meshContainer)
        {
            DrawCanvas(rect);
            DrawTexture(rect, meshContainer.Texture);
            DrawMesh(rect, meshContainer.Mesh);
        }

        protected virtual void DrawCanvas(Rect rect)
        {
            if (Event.current.type == EventType.Layout)
            {
                return;
            }

            Material canvasGridMaterial = EditorAssets.MaterialInstances.CanvasGrid;
            canvasGridMaterial.SetVector(Ids.MaterialProperties.RectSize, new Vector2(rect.width, rect.height));
            EditorGUI.DrawPreviewTexture(rect, EditorAssets.Textures.CanvasGrid, canvasGridMaterial, ScaleMode.ScaleAndCrop);
        }

        protected virtual void DrawTexture(Rect position, Texture2D texture)
        {
            Material material = EditorAssets.MaterialInstances.Additive;
            material.SetTexture("_MainTex", texture);
            EditorGUI.DrawPreviewTexture(position, texture, material, ScaleMode.StretchToFill);
            // GUI.DrawTexture(position, texture, ScaleMode.StretchToFill, true);
            // GUI.Draw
            // GuiUtility.Draw(position, material);
        }

        protected virtual void DrawMesh(Rect position, Mesh mesh)
        {
            if (Event.current.type == EventType.Layout)
            {
                return;
            }

            Handles.BeginGUI();
            using (new Handles.DrawingScope())
            {
                Handles.color = Color.white;
                int triangleCount = mesh.triangles.Length / 3;
                for (int i = 0; i < triangleCount; i++)
                {
                    int startIndex = i * 3;
                    TriangleVertexCache[0] = ConvertPoint(mesh.vertices[mesh.triangles[startIndex]]);
                    TriangleVertexCache[1] = ConvertPoint(mesh.vertices[mesh.triangles[startIndex + 1]]);
                    TriangleVertexCache[2] = ConvertPoint(mesh.vertices[mesh.triangles[startIndex + 2]]);
                    Handles.DrawLines(TriangleVertexCache, TriangleLineIndices);
                }
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
    }
}
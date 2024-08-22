using System;
using MichisMeshMakers.Editor.Assets;
using MichisMeshMakers.Editor.Extensions;
using UnityEditor;
using UnityEngine;

namespace MichisMeshMakers.Editor.OctagonCreator
{
    [Serializable]
    public class OctagonCreatorCanvas
    {
        private Texture2D _texture;

        public void SetTexture(Texture2D texture)
        {
            _texture = texture;
        }

        public void Draw(Rect canvasRect)
        {
            var canvasGridMaterial = EditorAssets.MaterialInstances.CanvasGrid;
            canvasGridMaterial.SetVector(Ids.MaterialProperties.RectSize, canvasRect.size);

            if (Event.current.type == EventType.Repaint)
            {
                EditorGUI.DrawPreviewTexture(canvasRect, EditorAssets.Textures.CanvasGrid, canvasGridMaterial, ScaleMode.StretchToFill);

                if (_texture != null)
                {
                    Rect textureRect = canvasRect.Center(_texture.GetSize());
                    var additive = EditorAssets.MaterialInstances.Additive;
                    additive.SetTexture("_MainTex", _texture);
                    EditorGUI.DrawPreviewTexture(textureRect, _texture, additive, ScaleMode.StretchToFill);
                }
            }

            if (Event.current.type != EventType.Layout)
            {
                Handles.BeginGUI();

                DrawHandles();

                Handles.EndGUI();
            }
        }

        private static void DrawHandles()
        {
            var p1 = new Vector3(50, 50);
            var p2 = new Vector3(150, 50);
            var p3 = new Vector3(150, 150);
            var p4 = new Vector3(50, 150);

            // Draw the quad
            Handles.color = Color.green;
            Handles.DrawAAConvexPolygon(p1, p2, p3, p4);
        }
    }
}
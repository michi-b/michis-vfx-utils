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

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.PropertyField(_textureProperty, MeshContainerLabels.SourceTexture);

            DrawBeforePreview();

            using (new GUILayout.VerticalScope())
            {
                float width = EditorGUIUtility.currentViewWidth - 35;
                Rect previewRect = GUILayoutUtility.GetRect(width, width, GUILayout.ExpandWidth(false));

                if (Event.current.type == EventType.Repaint)
                {
                    Material canvasGridMaterial = Assets.MaterialInstances.CanvasGrid;
                    canvasGridMaterial.SetVector(Ids.MaterialProperties.RectSize, new Vector2(width, width));
                    EditorGUI.DrawPreviewTexture(previewRect, Assets.Textures.CanvasGrid, canvasGridMaterial, ScaleMode.ScaleAndCrop);
                }
            }
        }

        protected virtual void DrawBeforePreview()
        {
        }
    }
}
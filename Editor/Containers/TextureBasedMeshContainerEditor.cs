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

        protected override void DrawMeshPreview(Rect rect)
        {
            Material canvasGridMaterial = Assets.MaterialInstances.CanvasGrid;
            canvasGridMaterial.SetVector(Ids.MaterialProperties.RectSize, new Vector2(rect.width, rect.height));
            EditorGUI.DrawPreviewTexture(rect, Assets.Textures.CanvasGrid, canvasGridMaterial, ScaleMode.ScaleAndCrop);
        }
    }
}
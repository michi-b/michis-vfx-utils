using UnityEditor;

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
        }
    }
}
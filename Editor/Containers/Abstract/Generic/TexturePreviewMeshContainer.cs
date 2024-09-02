using UnityEngine;
using UnityEngine.Serialization;

namespace MichisVfxUtils.Editor.Containers.Abstract.Generic
{
    public abstract class TexturePreviewMeshContainer : MeshContainer
    {
        public const string PreviewSettingsFieldName = nameof(_previewSettings);

        [SerializeField] private TexturePreviewSettings _previewSettings;

        // maximum extent of vertices (positive and negative)
        protected const float Limit = 0.5f;

        public TexturePreviewSettings PreviewSettings => _previewSettings;
    }
}
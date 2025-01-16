using System;
using Michis.VfxUtils.Editor.Assets;
using UnityEngine;
using UnityEngine.Serialization;

namespace Michis.VfxUtils.Editor.Containers.Abstract.Generic
{
    [Serializable]
    public struct TexturePreviewSettings
    {
        public const string TextureFieldName = nameof(_texture);
        public const string TexturePreviewMaterialFieldName = nameof(_material);

        [SerializeField] private Texture2D _texture;

        [FormerlySerializedAs("_previewMaterial")] [SerializeField]
        private TexturePreviewMaterial _material;

        public Texture2D Texture => _texture;
        public TexturePreviewMaterial Material => _material;
    }
}
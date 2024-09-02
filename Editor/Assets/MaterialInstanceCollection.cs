using System;
using UnityEngine;

namespace MichisVfxUtils.Editor.Assets
{
    [Serializable]
    public class MaterialInstanceCollection
    {
        [SerializeField] private Material _canvasGrid;
        [SerializeField] private Material _additive;
        [SerializeField] private Material _transparent;

        private Material _canvasGridInstance;
        private Material _additiveInstance;
        private Material _transparentInstance;

        public Material GetCanvasGrid(Rect rect)
        {
            // ReSharper disable once Unity.PerformanceCriticalCodeNullComparison
            if (_canvasGridInstance == null)
            {
                _canvasGridInstance = new Material(_canvasGrid);
            }

            _canvasGridInstance.SetVector(Ids.MaterialProperties.RectSize, new Vector2(rect.width, rect.height));

            return _canvasGridInstance;
        }

        public Material GetTexturePreviewMaterial(TexturePreviewMaterial texturePreviewMaterial, Texture2D texture)
        {
            return texturePreviewMaterial switch
            {
                TexturePreviewMaterial.Transparent => GetTransparent(texture),
                TexturePreviewMaterial.Additive => GetAdditive(texture),
                _ => throw new ArgumentOutOfRangeException(nameof(texturePreviewMaterial), texturePreviewMaterial, null)
            };
        }

        public void ClearCache()
        {
            _canvasGridInstance = null;
            _additiveInstance = null;
        }

        private Material GetTransparent(Texture texture)
        {
            // ReSharper disable once Unity.PerformanceCriticalCodeNullComparison
            if (_transparentInstance == null)
            {
                _transparentInstance = new Material(_transparent);
            }

            _transparentInstance.SetTexture(Ids.MaterialProperties.MainTexture, texture);

            return _transparentInstance;
        }

        private Material GetAdditive(Texture texture)
        {
            // ReSharper disable once Unity.PerformanceCriticalCodeNullComparison
            if (_additiveInstance == null)
            {
                _additiveInstance = new Material(_additive);
            }

            _additiveInstance.SetTexture(Ids.MaterialProperties.MainTexture, texture);

            return _additiveInstance;
        }
    }
}
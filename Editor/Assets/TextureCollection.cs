using System;
using UnityEngine;

namespace MichisUnityVfxUtilities.MichisUnityVfxUtilities.Editor.Assets
{
    [Serializable]
    public class TextureCollection
    {
        [SerializeField] private Texture2D _canvasGrid;

        public Texture2D CanvasGrid => _canvasGrid;
    }
}
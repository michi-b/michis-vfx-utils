using System;
using UnityEngine;

namespace MichisMeshMakers.Editor.Assets
{
    [Serializable]
    public class TextureCollection
    {
        [SerializeField] private Texture2D _canvasGrid;

        public Texture2D CanvasGrid => _canvasGrid;
    }
}
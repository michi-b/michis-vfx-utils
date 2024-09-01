using UnityEngine;

namespace MichisUnityVfxUtilities.MichisUnityVfxUtilities.Editor.Containers.Abstract.Generic
{
    public abstract class TextureBasedMeshContainer : MeshContainer
    {
        public const string TextureFieldName = nameof(_texture);

        // maximum extent of vertices (positive and negative)
        protected const float Limit = 0.5f;

        [SerializeField] private Texture2D _texture;

        public Texture2D Texture => _texture;
    }
}
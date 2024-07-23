using UnityEngine;

namespace MichisMeshMakers
{
    public class TextureCanvas
    {
        private Vector2 _offset = Vector2.zero;
        private Vector2 _size;
        private Texture2D _texture;
        private Vector2 _zoom = Vector2.one;

        public TextureCanvas(Texture2D texture, Vector2 size)
        {
            _texture = texture;
            _size = size;
        }
    }
}
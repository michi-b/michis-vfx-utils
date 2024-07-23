using UnityEngine;

namespace TextureMeshEditor
{
    public class Canvas
    {
        private Texture2D _texture;
        private Vector2 _size;
        private Vector2 _zoom = Vector2.one;
        private Vector2 _offset = Vector2.zero;

        public Canvas(Texture2D texture, Vector2 size)
        {
            _texture = texture;
            _size = size;
        }
    }
}
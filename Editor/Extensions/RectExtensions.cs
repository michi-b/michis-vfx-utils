using UnityEngine;

namespace MichisMeshMakers.Editor.Extensions
{
    public static class RectExtensions
    {
        public static Rect Center(this Rect container, Vector2 size)
        {
            return new Rect(container.center - size * 0.5f, size);
        }
    }
}
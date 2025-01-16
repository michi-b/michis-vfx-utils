using UnityEngine;

namespace Michis.VfxUtils.Editor.Extensions
{
    public static class Texture2DExtensions
    {
        public static Vector2 GetSize(this Texture2D texture)
        {
            return new Vector2(texture.width, texture.height);
        }
    }
}
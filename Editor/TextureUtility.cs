using UnityEngine;

namespace MichisMeshMakers.Editor
{
    public static class TextureUtility
    {
        public static Texture2D CreateSinglePixel(Color color)
        {
            var pix = new Color[1];
            pix[0] = color;
            var result = new Texture2D(1, 1);
            result.SetPixels(pix);
            result.Apply();
            return result;
        }
    }
}
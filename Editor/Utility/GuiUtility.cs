using UnityEngine;

namespace MichisMeshMakers.Editor.Utility
{
    public static class GuiUtility
    {
        public static void Draw(Rect rect, Material material)
        {
            if (Event.current.type == EventType.Repaint)
            {
                // Set up the material pass
                material.SetPass(0);

                // Draw a quad using GL to render the texture
                GL.PushMatrix();
                GL.LoadOrtho();
                GL.Begin(GL.QUADS);

                // Convert rect coordinates to normalized device coordinates
                float x1 = rect.x / Screen.width;
                float y1 = rect.y / Screen.height;
                float x2 = (rect.x + rect.width) / Screen.width;
                float y2 = (rect.y + rect.height) / Screen.height;

                GL.TexCoord2(0, 0); GL.Vertex3(x1, y1, 0);
                GL.TexCoord2(1, 0); GL.Vertex3(x2, y1, 0);
                GL.TexCoord2(1, 1); GL.Vertex3(x2, y2, 0);
                GL.TexCoord2(0, 1); GL.Vertex3(x1, y2, 0);

                GL.End();
                GL.PopMatrix();
            }
        }
    }
}
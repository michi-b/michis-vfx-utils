using UnityEditor;
using UnityEngine;

namespace MichisMeshMakers.Editor
{
    public class OctagonCreator : Window
    {
        private const string Title = "Octagon Creator";

        private TextureCanvas _canvas;

        private Texture2D _texture;

        protected virtual void OnGUI()
        {
            using var horizontalScope = new EditorGUILayout.HorizontalScope();
            DrawSettings();
            var canvasRect = GUILayoutUtility.GetRect(GUIContent.none, Styles.Default, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            EditorGUI.LabelField(canvasRect, $"Rect: {{{canvasRect.width},{canvasRect.height}}}");

            canvasRect.position = canvasRect.position + new Vector2(10f, 10f);
            canvasRect.size = canvasRect.size - new Vector2(20f, 20f);
            
            if (Event.current.type != EventType.Repaint)
            {
                return;
            }

            Assets.Materials.White.SetPass(0);

            Vector3 p1 = new Vector3(50, 50);
            Vector3 p2 = new Vector3(150, 50);
            Vector3 p3 = new Vector3(150, 150);
            Vector3 p4 = new Vector3(50, 150);

            // Draw the quad
            Handles.BeginGUI();
            Handles.color = Color.green;
            Handles.DrawAAConvexPolygon(p1, p2, p3, p4);
            Handles.Draw   
            Handles.EndGUI();
            
            // GL.PushMatrix();
            // GL.LoadPixelMatrix();
            // GL.Begin(GL.QUADS);
            // GL.Vertex3(canvasRect.x, canvasRect.y, 0);
            // GL.Vertex3(canvasRect.x, canvasRect.y + canvasRect.height, 0);
            // GL.Vertex3(canvasRect.x + canvasRect.width, canvasRect.y + canvasRect.height, 0);
            // GL.Vertex3(canvasRect.x + canvasRect.width, canvasRect.y, 0);
            // GL.End();
            // GL.PopMatrix();
        }

        [MenuItem(Menu.WindowPath + Title)]
        public new static void Show()
        {
            GetWindow<OctagonCreator>(Title);
        }

        private void DrawSettings()
        {
            using var settingsScope = CreateSettingsScope();
            EditorGUILayout.LabelField("Settings", EditorStyles.boldLabel);
            _texture = (Texture2D)EditorGUILayout.ObjectField("Target Texture", _texture, typeof(Texture2D), false);
        }
    }
}
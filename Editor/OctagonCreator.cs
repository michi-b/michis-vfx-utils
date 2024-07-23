using UnityEditor;
using UnityEngine;

namespace MichisMeshMakers
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
        }

        [MenuItem(Menu.WindowPath + Title)]
        public static void ShowWindow()
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
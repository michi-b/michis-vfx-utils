using UnityEditor;
using UnityEngine;

namespace MichisMeshMakers.Editor.OctagonCreator
{
    public class OctagonCreatorWindow : Window
    {
        private const string Title = "Octagon Creator";

        private OctagonCreatorCanvas _canvas;

        private Texture2D _texture;

        protected override void OnEnable()
        {
            base.OnEnable();
            _canvas = new OctagonCreatorCanvas();
            ApplyTexture();
        }

        protected virtual void OnGUI()
        {
            using var horizontalScope = new EditorGUILayout.HorizontalScope();
            DrawSettings();
            var canvasRect = GUILayoutUtility.GetRect(GUIContent.none, Styles.Default, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

            _canvas.Draw(canvasRect);
        }

        [MenuItem(Menu.WindowPath + Title)]
        public new static void Show()
        {
            GetWindow<OctagonCreatorWindow>(Title);
        }

        private void DrawSettings()
        {
            using var settingsScope = CreateSettingsScope();
            EditorGUILayout.LabelField("Settings", EditorStyles.boldLabel);

            EditorGUI.BeginChangeCheck();
            _texture = (Texture2D)EditorGUILayout.ObjectField("Target Texture", _texture, typeof(Texture2D), false);
            if (EditorGUI.EndChangeCheck())
            {
                ApplyTexture();
            }
        }

        private void ApplyTexture()
        {
            _canvas.SetTexture(_texture);
        }
    }
}
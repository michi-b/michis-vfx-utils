using System;
using UnityEditor;
using UnityEngine;

namespace TextureMeshEditor
{
    public class Window : EditorWindow
    {
        [MenuItem("Window/Texture Mesh Editor")]
        public static void ShowWindow()
        {
            var window = GetWindow<Window>("Texture Mesh Editor");
        }

        protected virtual void OnEnable()
        {
            _settingsStyle = new GUIStyle
            {
                normal = { background = TextureUtility.CreateSinglePixel(new Color(0.1f, 0.1f, 0.1f, 0.5f)) }
            };
            _canvasStyle = new GUIStyle();
        }

        protected virtual void OnDisable()
        {
            DestroyImmediate(_settingsStyle.normal.background);
            DestroyImmediate(_canvasStyle.normal.background);
        }

        protected virtual void OnGUI()
        {
            using var horizontalScope = new EditorGUILayout.HorizontalScope();
            DrawSettings();
            var canvasRect = GUILayoutUtility.GetRect(GUIContent.none, _canvasStyle, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            EditorGUI.LabelField(canvasRect, $"Rect: {{{canvasRect.width},{canvasRect.height}}}");
        }

        private void DrawSettings()
        {
            using var settingsScope = new EditorGUILayout.VerticalScope(_settingsStyle, GUILayout.Width(250f), GUILayout.ExpandHeight(true));
            using var scrollScope = new GUILayout.ScrollViewScope(_settingsScrollPosition, false, true, GUILayout.ExpandHeight(true));

            _settingsScrollPosition = scrollScope.scrollPosition;
            EditorGUILayout.LabelField("Settings", EditorStyles.boldLabel);
            _texture = (Texture2D)EditorGUILayout.ObjectField("Target Texture", _texture, typeof(Texture2D), false);
        }

        private Texture2D _texture;
        private GUIStyle _settingsStyle;
        private Vector2 _settingsScrollPosition;
        
        private GUIStyle _canvasStyle;
        private Canvas _canvas;
    }
}
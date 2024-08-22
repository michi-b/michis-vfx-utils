using System;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace MichisMeshMakers.Editor
{
    public abstract class MichisMeshMakerWindow : EditorWindow
    {
        private static int _windowCount;
        [PublicAPI] public static GuiStyles Styles { get; private set; }

        private Vector2 SettingsScrollPosition { get; set; }

        protected virtual void OnEnable()
        {
            _windowCount++;
            if (_windowCount == 1)
            {
                Styles = new GuiStyles();
            }
        }

        protected virtual void OnDisable()
        {
            _windowCount--;

            if (_windowCount > 0)
            {
                return;
            }

            Styles.Dispose();
            Styles = null;
        }

        protected SettingsScope CreateSettingsScope()
        {
            return new SettingsScope(this);
        }

        public class GuiStyles : IDisposable
        {
            private readonly Texture2D _transparentDarkTexture = TextureUtility.CreateSinglePixel(new Color(0.1f, 0.1f, 0.1f, 0.5f));

            public GuiStyles()
            {
                SettingsBlock = new GUIStyle
                {
                    normal = { background = _transparentDarkTexture }
                };
            }

            public GUIStyle Default { get; } = new GUIStyle();

            public GUIStyle SettingsBlock { get; }

            public void Dispose()
            {
                DestroyImmediate(_transparentDarkTexture);
            }
        }

        [PublicAPI]
        public class SettingsScope : IDisposable
        {
            private const float SettingsScopeWidth = 250f;
            private readonly GUILayout.ScrollViewScope _scrollScope;
            private readonly EditorGUILayout.VerticalScope _settingsScope;

            public SettingsScope(MichisMeshMakerWindow window)
            {
                _settingsScope = new EditorGUILayout.VerticalScope(Styles.SettingsBlock, GUILayout.Width(SettingsScopeWidth), GUILayout.ExpandHeight(true));
                EditorGUI.BeginChangeCheck();
                _scrollScope = new GUILayout.ScrollViewScope(window.SettingsScrollPosition, false, true, GUILayout.ExpandHeight(true));
                if (EditorGUI.EndChangeCheck())
                {
                    window.SettingsScrollPosition = _scrollScope.scrollPosition;
                }
            }

            public void Dispose()
            {
                _scrollScope.Dispose();
                _settingsScope.Dispose();
            }
        }
    }
}
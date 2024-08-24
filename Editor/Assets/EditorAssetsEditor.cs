using UnityEditor;
using UnityEngine;

namespace MichisMeshMakers.Editor.Assets
{
    [InitializeOnLoad]
    [CustomEditor(typeof(EditorAssets))]
    public class EditorAssetsEditor : UnityEditor.Editor
    {
        static EditorAssetsEditor()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange obj)
        {
            EditorAssets.Load().ReloadMaterialInstances(false);
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Reload Material Instances"))
            {
                var assets = (EditorAssets)target;
                assets.ReloadMaterialInstances();
            }
        }

        [MenuItem(Menu.Tools + "Reload Material Instances")]
        public static void ReloadMaterialInstances()
        {
            EditorAssets.Load().ReloadMaterialInstances();
        }
    }
}
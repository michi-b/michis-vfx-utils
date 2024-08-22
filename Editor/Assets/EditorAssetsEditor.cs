using UnityEditor;
using UnityEngine;

namespace MichisMeshMakers.Editor.Assets
{
    [CustomEditor(typeof(EditorAssets))]
    public class EditorAssetsEditor : UnityEditor.Editor
    {
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
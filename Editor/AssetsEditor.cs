using UnityEditor;
using UnityEngine;

namespace MichisMeshMakers.Editor
{
    [CustomEditor(typeof(Assets))]
    public class AssetsEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Reload Material Instances"))
            {
                var assets = (Assets) target;
                assets.ReloadMaterialInstances();
            }
        }
    }
}
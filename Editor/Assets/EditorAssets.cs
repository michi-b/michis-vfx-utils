using System;
using UnityEditor;
using UnityEngine;

namespace MichisVfxUtils.Editor.Assets
{
    public class EditorAssets : ScriptableObject
    {
        private static EditorAssets _instance;

        [SerializeField] private MaterialCollection _materials;
        [SerializeField] private MaterialInstanceCollection _materialInstances;
        [SerializeField] private TextureCollection _textures;

        private static EditorAssets Instance => _instance ??= Load();
        public static MaterialCollection Materials => Instance._materials;
        public static MaterialInstanceCollection MaterialInstances => Instance._materialInstances;
        public static TextureCollection Textures => Instance._textures;

        public static EditorAssets Load()
        {
            string[] paths = AssetDatabase.FindAssets($"t:{nameof(EditorAssets)}");

            foreach (string path in paths)
            {
                var assets = AssetDatabase.LoadAssetAtPath<EditorAssets>(AssetDatabase.GUIDToAssetPath(path));
                if (assets != null)
                {
                    return assets;
                }
            }

            throw new Exception($"{Menu.Name} could not find editor assets.");
        }

        public void ReloadMaterialInstances(bool logMessage = false)
        {
            _materialInstances.ClearCache();
            if (logMessage)
            {
                Debug.Log($"{Menu.Name} reloaded editor material instances.");
            }
        }
    }
}
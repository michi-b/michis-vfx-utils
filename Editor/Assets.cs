using System;
using UnityEditor;
using UnityEngine;

namespace MichisMeshMakers.Editor
{
    public class Assets : ScriptableObject
    {
        private static Assets _instance;

        [SerializeField] private MaterialCollection _materials;

        [SerializeField] private TextureCollection _textures;


        private static Assets Instance => _instance ??= Load();

        public static MaterialCollection Materials => Instance._materials;

        public static TextureCollection Textures => Instance._textures;

        private static Assets Load()
        {
            var paths = AssetDatabase.FindAssets($"t:{nameof(Assets)}");

            if (paths.Length != 1)
            {
                throw new Exception($"Expected 1 {typeof(Assets).FullName} but found {paths.Length}");
            }

            var path = paths[0];
            var result = AssetDatabase.LoadAssetAtPath<Assets>(AssetDatabase.GUIDToAssetPath(path));

            if (result == null)
            {
                throw new Exception($"Paths at {path} is not a {typeof(Assets).FullName}");
            }

            return result;
        }

        [Serializable]
        public class MaterialCollection
        {
            [SerializeField] private Material _white;
            [SerializeField] private Material _canvasGrid;

            public Material White => _white;

            public Material CanvasGrid => _canvasGrid;
        }

        [Serializable]
        public class TextureCollection
        {
            [SerializeField] private Texture2D _canvasGrid;

            public Texture2D CanvasGrid => _canvasGrid;
        }
    }
}
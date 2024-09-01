using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace MichisVfxUtils.Editor
{
    public class Ids
    {
        private static Ids _instance;

        private static Ids Instance => _instance ??= new Ids();

        private readonly MaterialPropertyIdCollection _materialProperties = new MaterialPropertyIdCollection();

        public static MaterialPropertyIdCollection MaterialProperties => Instance._materialProperties;

        [SuppressMessage("ReSharper", "ConvertToAutoProperty")]
        public class MaterialPropertyIdCollection
        {
            private const string RectSizeName = "_RectSize";

            private readonly int _rectSizeId = Shader.PropertyToID(RectSizeName);

            public int RectSize => _rectSizeId;
        }
    }
}
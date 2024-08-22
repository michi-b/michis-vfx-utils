using System;
using UnityEngine;

namespace MichisMeshMakers.Editor.Assets
{
    [Serializable]
    public class MaterialInstanceCollection
    {
        [SerializeField] private Material _canvasGrid;
        [SerializeField] private Material _additive;


        public Material CanvasGrid { get; private set; }

        public Material Additive { get; private set; }

        // ReSharper disable once MemberHidesStaticFromOuterClass
        public void Load()
        {
            CanvasGrid = new Material(_canvasGrid);
            Additive = new Material(_additive);
        }
    }
}
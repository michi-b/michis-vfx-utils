using System;
using UnityEngine;

namespace Michis.VfxUtils.Editor.Assets
{
    [Serializable]
    public class MaterialCollection
    {
        [SerializeField] private Material _white;

        public Material White => _white;
    }
}
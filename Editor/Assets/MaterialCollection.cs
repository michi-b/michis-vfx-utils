using System;
using UnityEngine;

namespace MichisMeshMakers.Editor.Assets
{
    [Serializable]
    public class MaterialCollection
    {
        [SerializeField] private Material _white;

        public Material White => _white;
    }
}
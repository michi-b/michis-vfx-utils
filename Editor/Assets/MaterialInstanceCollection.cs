using System;
using UnityEngine;

namespace MichisMeshMakers.Editor.Assets
{
    [Serializable]
    public class MaterialInstanceCollection
    {
        [SerializeField] private Material _canvasGrid;
        [SerializeField] private Material _additive;

        private Material _canvasGridInstance;
        private Material _additiveInstance;

        public Material CanvasGrid
        {
            get
            {
                // ReSharper disable once Unity.PerformanceCriticalCodeNullComparison
                if (_canvasGridInstance == null)
                {
                    _canvasGridInstance = new Material(_canvasGrid);
                }

                return _canvasGridInstance;
            }
        }

        public Material Additive
        {
            get
            {
                // ReSharper disable once Unity.PerformanceCriticalCodeNullComparison
                if (_additiveInstance == null)
                {
                    _additiveInstance = new Material(_additive);
                }

                return _additiveInstance;
            }
        }

        // ReSharper disable once MemberHidesStaticFromOuterClass
        public void ClearCache()
        {
            _canvasGridInstance = null;
            _additiveInstance = null;
        }
    }
}
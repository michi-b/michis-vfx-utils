using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MichisMeshMakers.Editor.Containers
{
    public abstract class MeshContainer : ScriptableObject
    {
        public const string MeshFieldName = nameof(_mesh);
        
        [SerializeField] private Mesh _mesh;
        [SerializeField] private MeshStatistics _statistics;
        
        public Mesh Mesh => _mesh;
        
        public void Initialize(Object selection, Mesh childMesh)
        {
            _mesh = childMesh;
            Initialize(selection);
            Apply();
        }
        
        protected virtual void Initialize(Object selection){}

        public void Apply()
        {
            Regenerate();
            _statistics = new MeshStatistics(_mesh);
        }
        
        protected abstract void Regenerate();
    }
}
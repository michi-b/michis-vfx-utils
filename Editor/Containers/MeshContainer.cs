using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MichisMeshMakers.Editor.Containers
{
    public abstract class MeshContainer : ScriptableObject
    {
        public const string MeshFieldName = nameof(_mesh);
        
        [SerializeField] private Mesh _mesh;

        public Mesh Mesh => _mesh;
        
        public void Initialize(Object selection, Mesh childMesh)
        {
            _mesh = childMesh;
            Initialize(selection);
        }
        
        protected abstract void Initialize(Object selection);
    }
}
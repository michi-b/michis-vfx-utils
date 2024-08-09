using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MichisMeshMakers.Editor.Containers
{
    public class MeshContainer : ScriptableObject
    {
        public const string MeshFieldName = nameof(_mesh);
        
        [SerializeField] private Mesh _mesh;

        public virtual void Initialize(Object selection, Mesh childMesh)
        {
            _mesh = childMesh;
            _mesh.vertices = Array.Empty<Vector3>();
            _mesh.triangles = Array.Empty<int>();
            _mesh.RecalculateNormals();
            _mesh.RecalculateBounds();
        }
    }
}
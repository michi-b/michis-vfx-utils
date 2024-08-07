using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MichisMeshMakers.Editor.Containers
{
    public class MeshContainer : ScriptableObject
    {
        [SerializeField] private Mesh _mesh;

        public Mesh Mesh
        {
            get => _mesh;
            set => _mesh = value;
        }

        public virtual void Initialize(Object selection)
        {
            _mesh = new Mesh
            {
                name = "Generated Mesh",
                vertices = Array.Empty<Vector3>(),
                triangles = Array.Empty<int>()
            };
            _mesh.RecalculateNormals();
            _mesh.RecalculateBounds();
        }
    }
}
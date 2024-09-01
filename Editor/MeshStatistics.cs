using System;
using UnityEngine;

namespace MichisVfxUtils.Editor
{
    [Serializable]
    public struct MeshStatistics
    {
        public const string SurfaceAreaFieldName = "_surfaceArea";

        [SerializeField] private float _surfaceArea;

        public float SurfaceArea => _surfaceArea;

        public MeshStatistics(Mesh mesh) : this()
        {
            _surfaceArea = EvaluateSurfaceArea(mesh);
        }

        private static float EvaluateSurfaceArea(Mesh mesh)
        {
            var vertices = mesh.vertices;
            var triangles = mesh.triangles;
            var doubleSurfaceArea = 0f;
            for (var i = 0; i < triangles.Length; i += 3)
            {
                var a = vertices[triangles[i]];
                var b = vertices[triangles[i + 1]];
                var c = vertices[triangles[i + 2]];
                doubleSurfaceArea += Vector3.Cross(b - a, c - a).magnitude;
            }

            return doubleSurfaceArea / 2f;
        }
    }
}
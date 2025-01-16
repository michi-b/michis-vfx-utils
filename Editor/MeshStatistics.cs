using System;
using UnityEngine;

namespace Michis.VfxUtils.Editor
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
            Vector3[] vertices = mesh.vertices;
            int[] triangles = mesh.triangles;
            float doubleSurfaceArea = 0f;
            for (int i = 0; i < triangles.Length; i += 3)
            {
                Vector3 a = vertices[triangles[i]];
                Vector3 b = vertices[triangles[i + 1]];
                Vector3 c = vertices[triangles[i + 2]];
                doubleSurfaceArea += Vector3.Cross(b - a, c - a).magnitude;
            }

            return doubleSurfaceArea / 2f;
        }
    }
}
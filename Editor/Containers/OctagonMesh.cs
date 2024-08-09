using UnityEngine;
using UnityEngine.Rendering;

namespace MichisMeshMakers.Editor.Containers
{
    public class OctagonMesh : TextureBasedMeshContainer
    {
        public const string AxisLengthFieldName = "_axisLength";
        public const string DiagonalLengthFieldName = "_diagonalLength";

        [SerializeField] [Range(0f, 1f)] private float _axisLength = 1f;

        [SerializeField] [Range(0f, Constants.SqrtOf2)]
        private float _diagonalLength = 1f;

        private readonly Vector3[] _vertices = new Vector3[8]; // 8 corners for an octagon
        private readonly int[] _indices = new int[6 * 3]; // 6 faces with 3 vertices each

        protected override void Initialize()
        {
            GenerateMesh();
        }

        private void SetVertex(int index, float x, float y)
        {
            _vertices[index] = new Vector3(x, y, 0f);
        }

        private void GenerateMesh()
        {
            const int bottomLeft = 0;
            const int left = 1;
            const int topLeft = 2;
            const int top = 3;
            const int topRight = 4;
            const int right = 5;
            const int bottomRight = 6;
            const int bottom = 7;

            float axisDistance = _axisLength * 0.5f;
            float diagonalPerAxisDistance = _diagonalLength * 0.5f * Constants.OneBySqrtOf2;

            SetVertex(bottomLeft, -diagonalPerAxisDistance, -diagonalPerAxisDistance);
            SetVertex(left, -axisDistance, 0f);
            SetVertex(topLeft, -diagonalPerAxisDistance, diagonalPerAxisDistance);
            SetVertex(top, 0f, axisDistance);
            SetVertex(topRight, diagonalPerAxisDistance, diagonalPerAxisDistance);
            SetVertex(right, axisDistance, 0f);
            SetVertex(bottomRight, diagonalPerAxisDistance, -diagonalPerAxisDistance);
            SetVertex(bottom, 0f, -axisDistance);

            int i = 0;

            // inner top left
            _indices[i++] = bottomLeft;
            _indices[i++] = topLeft;
            _indices[i++] = topRight;

            // left
            _indices[i++] = bottomLeft;
            _indices[i++] = left;
            _indices[i++] = topLeft;

            // top
            _indices[i++] = topLeft;
            _indices[i++] = top;
            _indices[i++] = topRight;

            // inner bottom right
            _indices[i++] = topRight;
            _indices[i++] = bottomRight;
            _indices[i++] = bottomLeft;

            // right
            _indices[i++] = topRight;
            _indices[i++] = right;
            _indices[i++] = bottomRight;

            // bottom
            _indices[i++] = bottomRight;
            _indices[i++] = bottom;
            _indices[i++] = bottomLeft;

            Mesh.SetVertices(_vertices);
            Mesh.SetIndices(_indices, MeshTopology.Triangles, 0);
            Mesh.Optimize();
            Mesh.RecalculateBounds(MeshUpdateFlags.DontValidateIndices);
            Mesh.UploadMeshData(true);
        }
    }
}
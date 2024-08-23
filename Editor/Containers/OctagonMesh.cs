using UnityEngine;
using UnityEngine.Rendering;

namespace MichisMeshMakers.Editor.Containers
{
    public class OctagonMesh : TextureBasedMeshContainer
    {
        public const string AxisLengthFieldName = "_axisLength";
        public const string DiagonalLengthFieldName = "_diagonalLength";

        private const int SimpleOctagonVertexCount = 8;
        private const int SimpleOctagonIndexCount = 6 * 3;

        private static readonly Vector3[] Vertices = new Vector3[SimpleOctagonVertexCount]; // 8 corners for an octagon
        private static readonly int[] Indices = new int[SimpleOctagonIndexCount]; // 6 faces with 3 vertices each

        [SerializeField] [Range(0f, 1f)] private float _axisLength = 1f;

        [SerializeField] [Range(0f, Constants.SqrtOf2)]
        private float _diagonalLength = 1f;

        protected override void Initialize()
        {
            RegenerateMesh();
        }

        private void SetVertex(int index, float x, float y)
        {
            Vertices[index] = new Vector3(x, y, 0f);
        }

        public void RegenerateMesh()
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
            Indices[i++] = bottomLeft;
            Indices[i++] = topLeft;
            Indices[i++] = topRight;

            // left
            Indices[i++] = bottomLeft;
            Indices[i++] = left;
            Indices[i++] = topLeft;

            // top
            Indices[i++] = topLeft;
            Indices[i++] = top;
            Indices[i++] = topRight;

            // inner bottom right
            Indices[i++] = topRight;
            Indices[i++] = bottomRight;
            Indices[i++] = bottomLeft;

            // right
            Indices[i++] = topRight;
            Indices[i++] = right;
            Indices[i++] = bottomRight;

            // bottom
            Indices[i++] = bottomRight;
            Indices[i++] = bottom;
            Indices[i++] = bottomLeft;

            Mesh.SetVertices(Vertices, 0, SimpleOctagonVertexCount);
            Mesh.SetIndices(Indices, 0, SimpleOctagonIndexCount, MeshTopology.Triangles, 0);
            Mesh.Optimize();
            Mesh.UploadMeshData(true);
        }
    }
}
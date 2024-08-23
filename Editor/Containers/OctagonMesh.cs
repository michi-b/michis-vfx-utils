using UnityEngine;

namespace MichisMeshMakers.Editor.Containers
{
    public class OctagonMesh : TextureBasedMeshContainer
    {
        public const string AxisLengthFieldName = "_axisLength";
        public const string DiagonalLengthFieldName = "_diagonalLength";

        private static readonly Vector3[] Vertices = new Vector3[Simple.VertexCount];
        private static readonly int[] Indices = new int[Simple.IndexCount];

        [SerializeField] [Range(0f, 1f)] private float _axisLength = 1f;

        [SerializeField] [Range(0f, Constants.SqrtOf2)]
        private float _diagonalLength = 1f;

        protected override void Initialize()
        {
            RegenerateMesh();
        }

        private static void SetVertex(int index, float x, float y)
        {
            Vertices[index] = new Vector3(x, y, 0f);
        }

        public void RegenerateMesh()
        {
            bool isOverSizedStraight = _axisLength > 1f;
            bool isOverSeizedDiagonal = _diagonalLength > Constants.SqrtOf2;
            if (isOverSizedStraight)
            {
                if (isOverSeizedDiagonal)
                {
                    // generate full quad
                }
                else
                {
                    // generate over-sized straight
                }
            }
            else
            {
                if (isOverSeizedDiagonal)
                {
                    // generate over-sized diagonal
                }
                else
                {
                      Simple.Generate(this);
                }
            }

            Mesh.SetVertices(Vertices, 0, Simple.VertexCount);
            Mesh.SetIndices(Indices, 0, Simple.IndexCount, MeshTopology.Triangles, 0);
            Mesh.Optimize();
            Mesh.UploadMeshData(true);
        }

        private static class Simple
        {
            public const int VertexCount = 8;
            public const int IndexCount = 6 * 3;
            private const int BottomLeft = 0;
            private const int Left = 1;
            private const int TopLeft = 2;
            private const int Top = 3;
            private const int TopRight = 4;
            private const int Right = 5;
            private const int BottomRight = 6;
            private const int Bottom = 7;

            public static void Generate(OctagonMesh target)
            {
                float axisDistance = target._axisLength * 0.5f;
                float diagonalPerAxisDistance = target._diagonalLength * 0.5f * Constants.OneBySqrtOf2;
                
                SetVertex(BottomLeft, -diagonalPerAxisDistance, -diagonalPerAxisDistance);
                SetVertex(Left, -axisDistance, 0f);
                SetVertex(TopLeft, -diagonalPerAxisDistance, diagonalPerAxisDistance);
                SetVertex(Top, 0f, axisDistance);
                SetVertex(TopRight, diagonalPerAxisDistance, diagonalPerAxisDistance);
                SetVertex(Right, axisDistance, 0f);
                SetVertex(BottomRight, diagonalPerAxisDistance, -diagonalPerAxisDistance);
                SetVertex(Bottom, 0f, -axisDistance);

                int i = 0;

                // inner top left
                Indices[i++] = BottomLeft;
                Indices[i++] = TopLeft;
                Indices[i++] = TopRight;

                // left
                Indices[i++] = BottomLeft;
                Indices[i++] = Left;
                Indices[i++] = TopLeft;

                // top
                Indices[i++] = TopLeft;
                Indices[i++] = Top;
                Indices[i++] = TopRight;

                // inner bottom right
                Indices[i++] = TopRight;
                Indices[i++] = BottomRight;
                Indices[i++] = BottomLeft;

                // right
                Indices[i++] = TopRight;
                Indices[i++] = Right;
                Indices[i++] = BottomRight;

                // bottom
                Indices[i++] = BottomRight;
                Indices[i++] = Bottom;
                Indices[i++] = BottomLeft;
            }
        }
    }
}
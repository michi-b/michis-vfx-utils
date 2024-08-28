using System;
using MichisMeshMakers.Editor.Containers.Abstract;
using MichisMeshMakers.Editor.Containers.Abstract.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace MichisMeshMakers.Editor.Containers
{
    public class OctagonMesh : TextureBasedMeshContainer
    {
        public const string AxisLengthFieldName = nameof(_straightLength);
        public const string DiagonalLengthFieldName = nameof(_diagonalLength);

        private static readonly Vector3[] Vertices = new Vector3[Simple.VertexCount];
        private static readonly Vector2[] Uvs = new Vector2[Simple.VertexCount];
        private static readonly int[] Indices = new int[Simple.IndexCount];

        [FormerlySerializedAs("_axisLength")] [SerializeField] [Range(0f, 1f)]
        private float _straightLength = 1f;

        [SerializeField] [Range(0f, Constants.SqrtOf2)]
        private float _diagonalLength = 1f;

        private static void SetVertex(int index, float x, float y)
        {
            Vertices[index] = new Vector3(x, y, 0f);
        }

        private static void GenerateUv(int index)
        {
            Vector3 vertex = Vertices[index];
            Uvs[index] = new Vector2(vertex.x + 0.5f, vertex.y + 0.5f);
        }

        protected override void Regenerate()
        {
            Mesh.Clear();

            bool isOverSizedStraight = _straightLength > 1f;
            bool isOverSeizedDiagonal = _diagonalLength > Constants.SqrtOf2;

            if (Math.Abs(_diagonalLength * Constants.OneBySqrtOf2 - _straightLength) < float.Epsilon)
            {
                Quad.Generate(this);
            }
            else if (isOverSizedStraight)
            {
                if (isOverSeizedDiagonal)
                {
                    // generate full quad
                }
                // generate over-sized straight
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

            int vertexCount = Mesh.vertexCount;
            for (int i = 0; i < vertexCount; i++) GenerateUv(i);
            Mesh.SetUVs(0, Uvs, 0, vertexCount);

            Mesh.Optimize();
            Mesh.UploadMeshData(true);
        }

        private static class Quad
        {
            private const int VertexCount = 4;
            private const int IndexCount = 2 * Constants.VerticesPerTriangle;
            private const int BottomLeft = 0;
            private const int TopLeft = 1;
            private const int TopRight = 2;
            private const int BottomRight = 3;

            public static void Generate(OctagonMesh octagon)
            {
                float axisDistance = octagon._straightLength * 0.5f;

                SetVertex(BottomLeft, -axisDistance, -axisDistance);
                SetVertex(TopLeft, -axisDistance, axisDistance);
                SetVertex(TopRight, axisDistance, axisDistance);
                SetVertex(BottomRight, axisDistance, -axisDistance);

                int i = 0;
                Indices[i++] = BottomLeft;
                Indices[i++] = TopRight;
                Indices[i++] = BottomRight;
                Indices[i++] = BottomLeft;
                Indices[i++] = TopLeft;
                Indices[i++] = TopRight;

                octagon.Mesh.SetVertices(Vertices, 0, VertexCount);
                octagon.Mesh.SetIndices(Indices, 0, IndexCount, MeshTopology.Triangles, 0);
            }
        }

        private static class Simple
        {
            public const int VertexCount = 8;
            public const int IndexCount = 6 * Constants.VerticesPerTriangle;
            private const int BottomLeft = 0;
            private const int Left = 1;
            private const int TopLeft = 2;
            private const int Top = 3;
            private const int TopRight = 4;
            private const int Right = 5;
            private const int BottomRight = 6;
            private const int Bottom = 7;

            public static void Generate(OctagonMesh octagon)
            {
                float axisDistance = octagon._straightLength * 0.5f;
                float diagonalPerAxisDistance = octagon._diagonalLength * 0.5f * Constants.OneBySqrtOf2;

                SetVertex(BottomLeft, -diagonalPerAxisDistance, -diagonalPerAxisDistance);
                SetVertex(Left, -axisDistance, 0f);
                SetVertex(TopLeft, -diagonalPerAxisDistance, diagonalPerAxisDistance);
                SetVertex(Top, 0f, axisDistance);
                SetVertex(TopRight, diagonalPerAxisDistance, diagonalPerAxisDistance);
                SetVertex(Right, axisDistance, 0f);
                SetVertex(BottomRight, diagonalPerAxisDistance, -diagonalPerAxisDistance);
                SetVertex(Bottom, 0f, -axisDistance);

                int i = 0;

                if (octagon._straightLength >= octagon._diagonalLength) // cross shape
                {
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
                else // x shape
                {
                    // inner left
                    Indices[i++] = Bottom;
                    Indices[i++] = Left;
                    Indices[i++] = Top;
                    // bottom left
                    Indices[i++] = Bottom;
                    Indices[i++] = BottomLeft;
                    Indices[i++] = Left;
                    // top left
                    Indices[i++] = Left;
                    Indices[i++] = TopLeft;
                    Indices[i++] = Top;
                    // inner right
                    Indices[i++] = Top;
                    Indices[i++] = Right;
                    Indices[i++] = Bottom;
                    // top right
                    Indices[i++] = Top;
                    Indices[i++] = TopRight;
                    Indices[i++] = Right;
                    // bottom right
                    Indices[i++] = Right;
                    Indices[i++] = BottomRight;
                    Indices[i++] = Bottom;
                }

                octagon.Mesh.SetVertices(Vertices, 0, VertexCount);
                octagon.Mesh.SetIndices(Indices, 0, IndexCount, MeshTopology.Triangles, 0);
            }
        }
    }
}
using System;
using MichisMeshMakers.Editor.Containers.Abstract.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace MichisMeshMakers.Editor.Containers
{
    public class OctagonMesh : TextureBasedMeshContainer
    {
        public const string AxisLengthFieldName = nameof(_straightLength);
        public const string DiagonalLengthFieldName = nameof(_diagonalLength);

        [FormerlySerializedAs("_axisLength")] [SerializeField] [Range(0f, 1f)]
        private float _straightLength = 1f;

        [SerializeField] [Range(0f, Constants.SqrtOf2)]
        private float _diagonalLength = 1f;

        protected override void Regenerate()
        {
            // bool isOverSizedStraight = _straightLength > 1f;
            // bool isOverSeizedDiagonal = _diagonalLength > Constants.SqrtOf2;

            MeshData meshData = Math.Abs(_diagonalLength * Constants.OneBySqrtOf2 - _straightLength) < float.Epsilon
                ? Quad.Generate(this)
                : Simple.Generate(this);
            try
            {
                int vertexCount = meshData.Vertices.Length;
                var uvs = new NativeArray<Vector2>(vertexCount, Allocator.Temp);
                var colors = new NativeArray<Color32>(vertexCount, Allocator.Temp);
                try
                {
                    uvs[0] = new Vector2();
                    for (int i = 0; i < vertexCount; i++)
                    {
                        Vector3 vertex = meshData.Vertices[i];
                        uvs[i] = new Vector2(vertex.x + 0.5f, vertex.y + 0.5f);
                        colors[i] = new Color32(255, 255, 255, 255);
                    }

                    Mesh m = Mesh;
                    m.Clear();
                    m.SetVertices(meshData.Vertices);
                    m.SetUVs(0, uvs);
                    m.SetColors(colors);

                    m.uv2 = m.uv3 = m.uv4 = m.uv5 = m.uv6 = m.uv7 = m.uv8 = null;
                    m.normals = null;
                    m.tangents = null;

                    m.SetIndices(meshData.Indices, MeshTopology.Triangles, 0);

                    m.Optimize();
                    //todo: try marking no longer readable, once memory leak is fixed
                    m.UploadMeshData(false);
                }
                finally
                {
                    uvs.Dispose();
                    colors.Dispose();
                }
            }
            finally
            {
                meshData.Dispose();
            }
        }

        private struct MeshData
        {
            public NativeArray<Vector3> Vertices;
            public NativeArray<int> Indices;

            public static MeshData Allocate(int vertexCount, int indexCount)
            {
                return new MeshData
                {
                    Vertices = new NativeArray<Vector3>(vertexCount, Allocator.Temp),
                    Indices = new NativeArray<int>(indexCount, Allocator.Temp)
                };
            }

            public void SetVertex(int index, float x, float y)
            {
                Vertices[index] = new Vector3(x, y, 0f);
            }

            public void Dispose()
            {
                Vertices.Dispose();
                Indices.Dispose();
            }
        }

        private static class Quad
        {
            private const int VertexCount = 4;
            private const int IndexCount = 2 * Constants.VerticesPerTriangle;
            private const int BottomLeft = 0;
            private const int TopLeft = 1;
            private const int TopRight = 2;
            private const int BottomRight = 3;

            public static MeshData Generate(OctagonMesh octagon)
            {
                MeshData result = MeshData.Allocate(VertexCount, IndexCount);

                float axisDistance = octagon._straightLength * 0.5f;

                result.SetVertex(BottomLeft, -axisDistance, -axisDistance);
                result.SetVertex(TopLeft, -axisDistance, axisDistance);
                result.SetVertex(TopRight, axisDistance, axisDistance);
                result.SetVertex(BottomRight, axisDistance, -axisDistance);

                result.Indices[0] = BottomLeft;
                result.Indices[1] = TopRight;
                result.Indices[2] = BottomRight;
                result.Indices[3] = BottomLeft;
                result.Indices[4] = TopLeft;
                result.Indices[5] = TopRight;

                return result;
            }
        }

        private static class Simple
        {
            private const int VertexCount = 8;
            private const int IndexCount = 6 * Constants.VerticesPerTriangle;
            private const int BottomLeft = 0;
            private const int Left = 1;
            private const int TopLeft = 2;
            private const int Top = 3;
            private const int TopRight = 4;
            private const int Right = 5;
            private const int BottomRight = 6;
            private const int Bottom = 7;

            public static MeshData Generate(OctagonMesh octagon)
            {
                MeshData result = MeshData.Allocate(VertexCount, IndexCount);

                float axisDistance = octagon._straightLength * 0.5f;
                float diagonalPerAxisDistance = octagon._diagonalLength * 0.5f * Constants.OneBySqrtOf2;

                result.SetVertex(BottomLeft, -diagonalPerAxisDistance, -diagonalPerAxisDistance);
                result.SetVertex(Left, -axisDistance, 0f);
                result.SetVertex(TopLeft, -diagonalPerAxisDistance, diagonalPerAxisDistance);
                result.SetVertex(Top, 0f, axisDistance);
                result.SetVertex(TopRight, diagonalPerAxisDistance, diagonalPerAxisDistance);
                result.SetVertex(Right, axisDistance, 0f);
                result.SetVertex(BottomRight, diagonalPerAxisDistance, -diagonalPerAxisDistance);
                result.SetVertex(Bottom, 0f, -axisDistance);

                if (octagon._straightLength >= octagon._diagonalLength) // cross shape
                {
                    // inner top left
                    result.Indices[0] = BottomLeft;
                    result.Indices[1] = TopLeft;
                    result.Indices[2] = TopRight;
                    // left
                    result.Indices[3] = BottomLeft;
                    result.Indices[4] = Left;
                    result.Indices[5] = TopLeft;
                    // top
                    result.Indices[6] = TopLeft;
                    result.Indices[7] = Top;
                    result.Indices[8] = TopRight;
                    // inner bottom right
                    result.Indices[9] = TopRight;
                    result.Indices[10] = BottomRight;
                    result.Indices[11] = BottomLeft;
                    // right
                    result.Indices[12] = TopRight;
                    result.Indices[13] = Right;
                    result.Indices[14] = BottomRight;
                    // bottom
                    result.Indices[15] = BottomRight;
                    result.Indices[16] = Bottom;
                    result.Indices[17] = BottomLeft;
                }
                else // x shape
                {
                    // inner left
                    result.Indices[0] = Bottom;
                    result.Indices[1] = Left;
                    result.Indices[2] = Top;
                    // bottom left
                    result.Indices[3] = Bottom;
                    result.Indices[4] = BottomLeft;
                    result.Indices[5] = Left;
                    // top left
                    result.Indices[6] = Left;
                    result.Indices[7] = TopLeft;
                    result.Indices[8] = Top;
                    // inner right
                    result.Indices[9] = Top;
                    result.Indices[10] = Right;
                    result.Indices[11] = Bottom;
                    // top right
                    result.Indices[12] = Top;
                    result.Indices[13] = TopRight;
                    result.Indices[14] = Right;
                    // bottom right
                    result.Indices[15] = Right;
                    result.Indices[16] = BottomRight;
                    result.Indices[17] = Bottom;
                }

                return result;
            }
        }
    }
}
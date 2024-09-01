using System;
using MichisVfxUtils.Editor.Containers.Abstract.Generic;
using MichisVfxUtils.Editor.Extensions;
using Unity.Collections;
using UnityEngine;

namespace MichisVfxUtils.Editor.Containers
{
    public class OctagonMesh : TextureBasedMeshContainer
    {
        public const string AxisLengthFieldName = nameof(_axisLength);
        public const string DiagonalLengthFieldName = nameof(_diagonalLength);
        public const string ConstrainLengthsFieldName = nameof(_constrainLengths);

        private const float AxisLengthConstraint = 1f;
        private const float DiagonalLengthConstraint = Constants.SqrtOf2;

        [SerializeField] private bool _constrainLengths = true;

        [SerializeField] [Range(0f, AxisLengthConstraint)]
        private float _axisLength = 1f;

        [SerializeField] [Range(0f, DiagonalLengthConstraint)]
        private float _diagonalLength = 1f;

        protected override void Regenerate()
        {
            var constrainedLengths = new Lengths
            {
                Axis = Mathf.Min(_axisLength, AxisLengthConstraint),
                Diagonal = Mathf.Min(_diagonalLength, DiagonalLengthConstraint)
            };
            var unconstrainedLengths = new Lengths
            {
                Axis = _axisLength,
                Diagonal = _diagonalLength
            };

            bool isAxisOverSized = unconstrainedLengths.Axis >= AxisLengthConstraint;
            bool isDiagonallyOverSized = unconstrainedLengths.Diagonal >= DiagonalLengthConstraint;

            bool isQuad = Math.Abs(constrainedLengths.Diagonal * Constants.OneBySqrtOf2 - _axisLength) < float.Epsilon
                          || (isAxisOverSized && isDiagonallyOverSized);

            MeshData meshData = isQuad
                ? Quad.Generate(constrainedLengths)
                : _constrainLengths
                    ? Octagon.Generate(constrainedLengths)
                    : isAxisOverSized
                        ? PlusShape.Generate(unconstrainedLengths)
                        : isDiagonallyOverSized
                            ? XShape.Generate(unconstrainedLengths)
                            : Octagon.Generate(constrainedLengths);

            try
            {
                int vertexCount = meshData.Vertices.Length;
                var uvs = new NativeArray<Vector2>(vertexCount, Allocator.Temp);
                // var colors = new NativeArray<Color32>(vertexCount, Allocator.Temp);
                try
                {
                    uvs[0] = new Vector2();
                    for (int i = 0; i < vertexCount; i++)
                    {
                        Vector3 vertex = meshData.Vertices[i];
                        uvs[i] = new Vector2(vertex.x + 0.5f, vertex.y + 0.5f);
                        // colors[i] = new Color32(255, 255, 255, 255);
                    }

                    Mesh m = Mesh;
                    m.Clear();
                    m.SetVertices(meshData.Vertices);
                    m.SetUVs(0, uvs);

                    m.colors = null;
                    m.uv2 = m.uv3 = m.uv4 = m.uv5 = m.uv6 = m.uv7 = m.uv8 = null;
                    m.normals = null;
                    m.tangents = null;

                    m.SetIndices(meshData.Indices, MeshTopology.Triangles, 0);

                    m.Optimize();
                    //todo: try marking no longer readable, once memory leak is fixed
                    m.UploadMeshData(true);
                }
                finally
                {
                    uvs.Dispose();
                    // colors.Dispose();
                }
            }
            finally
            {
                meshData.Dispose();
            }
        }

        private struct Lengths
        {
            public float Axis;
            public float Diagonal;
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

            public static MeshData Generate(Lengths lengths)
            {
                MeshData result = MeshData.Allocate(VertexCount, IndexCount);

                float axisDistance = lengths.Axis * 0.5f;

                result.SetVertex(BottomLeft, -axisDistance, -axisDistance);
                result.SetVertex(TopLeft, -axisDistance, axisDistance);
                result.SetVertex(TopRight, axisDistance, axisDistance);
                result.SetVertex(BottomRight, axisDistance, -axisDistance);

                result.Indices[0] = BottomLeft;
                result.Indices[1] = TopLeft;
                result.Indices[2] = TopRight;
                result.Indices[3] = BottomLeft;
                result.Indices[4] = TopRight;
                result.Indices[5] = BottomRight;

                return result;
            }
        }

        private static class Octagon
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

            public static MeshData Generate(Lengths lengths)
            {
                MeshData result = MeshData.Allocate(VertexCount, IndexCount);

                float axisDistance = lengths.Axis * Limit;
                float diagonalPerAxisDistance = lengths.Diagonal * Limit * Constants.OneBySqrtOf2;

                result.SetVertex(BottomLeft, -diagonalPerAxisDistance, -diagonalPerAxisDistance);
                result.SetVertex(Left, -axisDistance, 0f);
                result.SetVertex(TopLeft, -diagonalPerAxisDistance, diagonalPerAxisDistance);
                result.SetVertex(Top, 0f, axisDistance);
                result.SetVertex(TopRight, diagonalPerAxisDistance, diagonalPerAxisDistance);
                result.SetVertex(Right, axisDistance, 0f);
                result.SetVertex(BottomRight, diagonalPerAxisDistance, -diagonalPerAxisDistance);
                result.SetVertex(Bottom, 0f, -axisDistance);

                if (lengths.Axis >= lengths.Diagonal) // cross shape
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

        private static class PlusShape
        {
            private const int VertexCount = 12;
            private const int IndexCount = 10 * Constants.VerticesPerTriangle;

            private const int BottomLeft = 0;
            private const int Left0 = 1;
            private const int Left1 = 2;
            private const int TopLeft = 3;
            private const int Top0 = 4;
            private const int Top1 = 5;
            private const int TopRight = 6;
            private const int Right0 = 7;
            private const int Right1 = 8;
            private const int BottomRight = 9;
            private const int Bottom0 = 10;
            private const int Bottom1 = 11;

            public static MeshData Generate(Lengths lengths)
            {
                MeshData result = MeshData.Allocate(VertexCount, IndexCount);

                float diagonalDistance = lengths.Diagonal * Limit * Constants.OneBySqrtOf2;

                result.SetVertex(BottomLeft, -diagonalDistance, -diagonalDistance);
                result.SetVertex(TopLeft, -diagonalDistance, diagonalDistance);
                result.SetVertex(TopRight, diagonalDistance, diagonalDistance);
                result.SetVertex(BottomRight, diagonalDistance, -diagonalDistance);

                Vector2 topRight = result.Vertices[TopRight].GetXy();
                var ray1 = new Ray2D(topRight, new Vector2(0, lengths.Axis * Limit) - topRight);
                var ray2 = new Ray2D(new Vector2(-Limit, Limit), Vector2.right);
                float shortAxisDistance = ray1.Intersect(ray2).x;

                result.SetVertex(Left0, -Limit, -shortAxisDistance);
                result.SetVertex(Left1, -Limit, shortAxisDistance);
                result.SetVertex(Top0, -shortAxisDistance, Limit);
                result.SetVertex(Top1, shortAxisDistance, Limit);
                result.SetVertex(Right0, Limit, shortAxisDistance);
                result.SetVertex(Right1, Limit, -shortAxisDistance);
                result.SetVertex(Bottom0, shortAxisDistance, -Limit);
                result.SetVertex(Bottom1, -shortAxisDistance, -Limit);

                result.Indices[0] = BottomLeft;
                result.Indices[1] = TopLeft;
                result.Indices[2] = TopRight;
                result.Indices[3] = BottomLeft;
                result.Indices[4] = TopRight;
                result.Indices[5] = BottomRight;

                result.Indices[6] = BottomLeft;
                result.Indices[7] = Left0;
                result.Indices[8] = TopLeft;
                result.Indices[9] = Left0;
                result.Indices[10] = Left1;
                result.Indices[11] = TopLeft;

                result.Indices[12] = TopLeft;
                result.Indices[13] = Top0;
                result.Indices[14] = TopRight;
                result.Indices[15] = Top0;
                result.Indices[16] = Top1;
                result.Indices[17] = TopRight;

                result.Indices[18] = TopRight;
                result.Indices[19] = Right0;
                result.Indices[20] = BottomRight;
                result.Indices[21] = Right0;
                result.Indices[22] = Right1;
                result.Indices[23] = BottomRight;

                result.Indices[24] = BottomRight;
                result.Indices[25] = Bottom0;
                result.Indices[26] = BottomLeft;
                result.Indices[27] = Bottom0;
                result.Indices[28] = Bottom1;
                result.Indices[29] = BottomLeft;

                return result;
            }
        }

        private static class XShape
        {
            private const int VertexCount = 16;
            private const int IndexCount = 14 * Constants.VerticesPerTriangle;

            private const int Bottom = 0;
            private const int BottomLeft0 = 1;
            private const int BottomLeft1 = 2;
            private const int BottomLeftTip = 3;
            private const int Left = 4;
            private const int TopLeft0 = 5;
            private const int TopLeft1 = 6;
            private const int TopLeftTip = 7;
            private const int Top = 8;
            private const int TopRight0 = 9;
            private const int TopRight1 = 10;
            private const int TopRightTip = 11;
            private const int Right = 12;
            private const int BottomRight0 = 13;
            private const int BottomRight1 = 14;
            private const int BottomRightTip = 15;

            public static MeshData Generate(Lengths lengths)
            {
                MeshData result = MeshData.Allocate(VertexCount, IndexCount);

                float axisDistance = lengths.Axis * Limit;

                result.SetVertex(Bottom, 0, -axisDistance);
                result.SetVertex(Left, -axisDistance, 0);
                result.SetVertex(Top, 0, axisDistance);
                result.SetVertex(Right, axisDistance, 0);

                float hypotheticalDiagonalAxisDistance = lengths.Diagonal * Limit * Constants.OneBySqrtOf2;
                var hypotheticalTopRight =
                    new Vector2(hypotheticalDiagonalAxisDistance, hypotheticalDiagonalAxisDistance);
                Vector2 top = result.Vertices[Top].GetXy();
                var ray1 = new Ray2D(top, hypotheticalTopRight - top);
                var ray2 = new Ray2D(new Vector2(-Limit, Limit), Vector2.right);
                float shortDiagonalAxisDistance = ray1.Intersect(ray2).x;

                result.SetVertex(BottomLeft0, -shortDiagonalAxisDistance, -Limit);
                result.SetVertex(BottomLeft1, -Limit, -shortDiagonalAxisDistance);
                result.SetVertex(BottomLeftTip, -Limit, -Limit);

                result.SetVertex(TopLeft0, -Limit, shortDiagonalAxisDistance);
                result.SetVertex(TopLeft1, -shortDiagonalAxisDistance, Limit);
                result.SetVertex(TopLeftTip, -Limit, Limit);

                result.SetVertex(TopRight0, shortDiagonalAxisDistance, Limit);
                result.SetVertex(TopRight1, Limit, shortDiagonalAxisDistance);
                result.SetVertex(TopRightTip, Limit, Limit);

                result.SetVertex(BottomRight0, Limit, -shortDiagonalAxisDistance);
                result.SetVertex(BottomRight1, shortDiagonalAxisDistance, -Limit);
                result.SetVertex(BottomRightTip, Limit, -Limit);

                result.Indices[0] = Bottom;
                result.Indices[1] = Left;
                result.Indices[2] = Top;
                result.Indices[3] = Bottom;
                result.Indices[4] = Top;
                result.Indices[5] = Right;

                result.Indices[6] = Bottom;
                result.Indices[7] = BottomLeft0;
                result.Indices[8] = BottomLeft1;
                result.Indices[9] = Bottom;
                result.Indices[10] = BottomLeft1;
                result.Indices[11] = Left;
                result.Indices[12] = BottomLeft0;
                result.Indices[13] = BottomLeftTip;
                result.Indices[14] = BottomLeft1;

                result.Indices[15] = Left;
                result.Indices[16] = TopLeft0;
                result.Indices[17] = TopLeft1;
                result.Indices[18] = Left;
                result.Indices[19] = TopLeft1;
                result.Indices[20] = Top;
                result.Indices[21] = TopLeft0;
                result.Indices[22] = TopLeftTip;
                result.Indices[23] = TopLeft1;

                result.Indices[24] = Top;
                result.Indices[25] = TopRight0;
                result.Indices[26] = TopRight1;
                result.Indices[27] = Top;
                result.Indices[28] = TopRight1;
                result.Indices[29] = Right;
                result.Indices[30] = TopRight0;
                result.Indices[31] = TopRightTip;
                result.Indices[32] = TopRight1;

                result.Indices[33] = Right;
                result.Indices[34] = BottomRight0;
                result.Indices[35] = BottomRight1;
                result.Indices[36] = Right;
                result.Indices[37] = BottomRight1;
                result.Indices[38] = Bottom;
                result.Indices[39] = BottomRight0;
                result.Indices[40] = BottomRightTip;
                result.Indices[41] = BottomRight1;

                return result;
            }
        }
    }
}
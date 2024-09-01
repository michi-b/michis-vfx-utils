using JetBrains.Annotations;
using UnityEngine;

namespace MichisMeshMakers.Editor
{
    public static class Arrays
    {
        public static class Temporary
        {
            [PublicAPI] public static readonly Vector3[] TriangleVertexCache = new Vector3[3];
        }

        [PublicAPI] public static readonly int[] TriangleLineIndices = { 0, 1, 1, 2, 2, 0 };
    }
}
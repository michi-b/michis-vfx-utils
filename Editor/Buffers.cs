using JetBrains.Annotations;
using UnityEngine;

namespace MichisUnityVfxUtilities.MichisUnityVfxUtilities.Editor
{
    public static class Buffers
    {
        public static class Temporary
        {
            [PublicAPI] public static readonly Vector3[] TriangleVectors3D = new Vector3[3];
        }

        [PublicAPI] public static readonly int[] TriangleLineIndices = { 0, 1, 1, 2, 2, 0 };
    }
}
using UnityEngine;

namespace MichisUnityVfxUtilities.MichisUnityVfxUtilities.Editor.Containers
{
    public static class MeshContainerGuiLabels
    {
        public static readonly GUIContent GeneratedMesh =
            new GUIContent("Generated mesh", "This asset's child mesh that it generates and updates.");

        public static readonly GUIContent SourceTexture = new GUIContent("Source texture",
            "The texture that this asset uses to generate and update the mesh.");

        public static readonly GUIContent
            Statistics = new GUIContent("Statistics", "Statistics of the generated mesh.");

        public static readonly GUIContent SurfaceArea =
            new GUIContent("Surface area", "The surface area of the generated mesh.");

        public static readonly GUIContent VertexCount =
            new GUIContent("Vertex count", "The vertex count of the generated mesh.");

        public static readonly GUIContent TriangleCount =
            new GUIContent("Triangle count", "The triangle count of the generated mesh.");
    }
}
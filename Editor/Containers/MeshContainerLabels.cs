using UnityEngine;

namespace MichisMeshMakers.Editor.Containers
{
    public static class MeshContainerLabels
    {
        public static readonly GUIContent GeneratedMesh = new GUIContent("Generated mesh", "This asset's child mesh that it generates and updates.");
        public static readonly GUIContent SourceTexture = new GUIContent("Source texture", "The texture that this asset uses to generate and update the mesh.");
    }
}
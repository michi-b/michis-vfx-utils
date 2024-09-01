using MichisUnityVfxUtilities.MichisUnityVfxUtilities.Editor.Containers;
using MichisUnityVfxUtilities.MichisUnityVfxUtilities.Editor.Utility;
using UnityEditor;
using UnityEngine;

namespace MichisUnityVfxUtilities.MichisUnityVfxUtilities.Editor
{
    public class ParticleSystemContextMenu : MonoBehaviour
    {
        [MenuItem(Menu.Context.ParticleSystem + "Create Octagon Mesh", true)]
        private static bool ValidateCreateOctagonMesh(MenuCommand menuCommand)
        {
            var particleSystem = (ParticleSystem)menuCommand.context;

            var assetPath = AssetDatabase.GetAssetPath(particleSystem);
            if (assetPath != null)
                if (!AssetDatabase.IsOpenForEdit(assetPath))
                    return false;

            var renderer = particleSystem.GetComponent<ParticleSystemRenderer>();
            if (renderer == null) return false;

            var material = renderer.sharedMaterial;
            if (material == null) return false;

            var texture = material.mainTexture as Texture2D;
            if (texture == null) return false;

            var texturePath = AssetDatabase.GetAssetPath(texture);
            // ReSharper disable once ConvertIfStatementToReturnStatement
            if (texturePath == null) return false;

            // check if the texture path is a read-only asset
            return AssetDatabaseUtility.GetIsInWritableFolder(texturePath);
        }

        [MenuItem(Menu.Context.ParticleSystem + "Create Octagon Mesh")]
        private static void CreateOctagonMesh(MenuCommand command)
        {
            var particleSystem = (ParticleSystem)command.context;
            var renderer = particleSystem.GetComponent<ParticleSystemRenderer>();
            var material = renderer.sharedMaterial;
            var texture = (Texture2D)material.mainTexture;
            var octagonMesh = OctagonMeshEditor.Create(texture);

            // assign new octagon mesh to particle system
            Undo.RecordObject(renderer, $"Assign Octagon Mesh to {renderer.gameObject.name}");
            if (renderer.renderMode != ParticleSystemRenderMode.Mesh)
                renderer.renderMode = ParticleSystemRenderMode.Mesh;

            renderer.mesh = octagonMesh.Mesh;
        }
    }
}
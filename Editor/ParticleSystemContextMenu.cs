using MichisVfxUtils.Editor.Containers;
using MichisVfxUtils.Editor.Utility;
using UnityEditor;
using UnityEngine;

namespace MichisVfxUtils.Editor
{
    public class ParticleSystemContextMenu : MonoBehaviour
    {
        [MenuItem(Menu.Context.ParticleSystem + "Create Octagon Mesh", true)]
        private static bool ValidateCreateOctagonMesh(MenuCommand menuCommand)
        {
            var particleSystem = (ParticleSystem)menuCommand.context;

            string assetPath = AssetDatabase.GetAssetPath(particleSystem);
            if (assetPath != null)
            {
                if (!AssetDatabase.IsOpenForEdit(assetPath))
                {
                    return false;
                }
            }

            var renderer = particleSystem.GetComponent<ParticleSystemRenderer>();
            if (renderer == null)
            {
                return false;
            }

            Material material = renderer.sharedMaterial;
            if (material == null)
            {
                return false;
            }

            var texture = material.mainTexture as Texture2D;
            if (texture == null)
            {
                return false;
            }

            string texturePath = AssetDatabase.GetAssetPath(texture);
            // ReSharper disable once ConvertIfStatementToReturnStatement
            if (texturePath == null)
            {
                return false;
            }

            // check if the texture path is a read-only asset
            return AssetDatabaseUtility.GetIsInWritableFolder(texturePath);
        }

        [MenuItem(Menu.Context.ParticleSystem + "Create Octagon Mesh")]
        private static void CreateOctagonMesh(MenuCommand command)
        {
            var particleSystem = (ParticleSystem)command.context;
            var renderer = particleSystem.GetComponent<ParticleSystemRenderer>();
            Material material = renderer.sharedMaterial;
            var texture = (Texture2D)material.mainTexture;
            OctagonMesh octagonMesh = OctagonMeshEditor.Create(texture);

            // assign new octagon mesh to particle system
            Undo.RecordObject(renderer, $"Assign Octagon Mesh to {renderer.gameObject.name}");
            if (renderer.renderMode != ParticleSystemRenderMode.Mesh)
            {
                renderer.renderMode = ParticleSystemRenderMode.Mesh;
            }

            renderer.mesh = octagonMesh.Mesh;
        }
    }
}
using System.IO;
using MichisMeshMakers.Editor.Utility;
using UnityEditor;
using UnityEngine;

namespace MichisMeshMakers.Editor
{
    public class ParticleSystemContextMenu : MonoBehaviour
    {
        [MenuItem(Menu.Context.ParticleSystem + "Create Octagon Mesh", true)]
        private static bool ValidateCreateOctagonMesh(MenuCommand menuCommand)
        {
            var particleSystem = (ParticleSystem)menuCommand.context;
            var renderer = particleSystem.GetComponent<ParticleSystemRenderer>();
            Material material = renderer.material;
            if (material == null)
            {
                return false; 
            }
            
            Texture texture = material.mainTexture;
            if(texture == null)
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
            ParticleSystem particleSystem = (ParticleSystem)command.context;
            ParticleSystemRenderer renderer = particleSystem.GetComponent<ParticleSystemRenderer>();
            Material material = renderer.material;
            Texture texture = material.mainTexture;
            string texturePath = AssetDatabase.GetAssetPath(texture);
            string path = AssetDatabase.GenerateUniqueAssetPath(Path.ChangeExtension(texturePath, "asset"));
            
        }
    }
}

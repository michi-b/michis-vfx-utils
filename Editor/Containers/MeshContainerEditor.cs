using System;
using System.IO;
using MichisMeshMakers.Editor.Utility;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MichisMeshMakers.Editor.Containers
{
    [CustomEditor(typeof(MeshContainer))]
    public class MeshContainerEditor<TMeshContainer> : UnityEditor.Editor where TMeshContainer : MeshContainer
    {
        public override void OnInspectorGUI()
        {
            // foreach (var targetObject in targets)
            // {
            //     var meshContainer = (MeshContainer) targetObject;
            //     if (meshContainer.Mesh == null)
            //     {
            //         Undo.RecordObject(meshContainer, "Create Initial Mesh");
            //         meshContainer.InitializeMesh();
            //         AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
            //     }
            // }
            base.OnInspectorGUI();
        }

        
        
        protected static TMeshContainer CreateAsset(string assetName, Func<string, Object> getCreationTarget)
        {
            var meshContainer = CreateInstance<TMeshContainer>();
            
            meshContainer.name = assetName;

            Object selection = Selection.activeObject;
            Object creationTarget = getCreationTarget(AssetDatabase.GetAssetPath(selection));

            string creationTargetName = creationTarget != null 
                ? Path.GetFileNameWithoutExtension(AssetDatabase.GetAssetPath(creationTarget)) 
                : null;
            
            string fileName = AssetDatabaseUtility.GetDependentFilename(creationTargetName, assetName);

            string path = AssetDatabaseUtility.GetCreateAssetPath(fileName);
            
            AssetDatabase.CreateAsset(meshContainer, path);

            var childMesh = new Mesh
            {
                vertices = Array.Empty<Vector3>(),
                triangles = Array.Empty<int>(),
                name = creationTargetName ?? assetName
            };

            // Save Child Mesh as a sub-asset of the parent asset
            AssetDatabase.AddObjectToAsset(childMesh, meshContainer);
            AssetDatabase.SaveAssets();

            // Link Child Mesh to Parent Asset
            Undo.RecordObject(meshContainer, "Initialize Mesh Container Mesh");
            meshContainer.Mesh = childMesh;
            meshContainer.Initialize(selection);

            // Save and refresh the AssetDatabase
            AssetDatabase.SaveAssets();
            AssetDatabaseUtility.ForceInstantRefresh();

            // Select the newly created Parent Asset in the Project Window
            Selection.activeObject = meshContainer;
            return meshContainer;
        }
    }
}
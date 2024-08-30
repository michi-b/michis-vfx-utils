using System;
using JetBrains.Annotations;
using MichisMeshMakers.Editor.Utility;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MichisMeshMakers.Editor.Containers.Abstract.Generic
{
    public abstract class MeshContainerEditor<TMeshContainer> : MeshContainerEditorBase where TMeshContainer : MeshContainer
    {
        [PublicAPI] protected TMeshContainer Target { get; private set; }

        [PublicAPI] protected TMeshContainer[] Targets { get; private set; }

        protected override void OnEnable()
        {
            base.OnEnable();
            Target = (TMeshContainer)target;
            Targets = Array.ConvertAll(targets, t => (TMeshContainer)t);
        }

        protected override void DrawMeshPreview(Rect previewRect, MeshContainer targetMeshContainer)
        {
            DrawMeshPreview(previewRect, (TMeshContainer)targetMeshContainer);
        }

        protected abstract void DrawMeshPreview(Rect rect, TMeshContainer meshContainer);

        protected static void Create(string assetName, Func<string, Object> getCreationTarget)
        {
            Object selection = Selection.activeObject;
            Object creationTarget = getCreationTarget(AssetDatabase.GetAssetPath(selection));

            assetName = creationTarget != null ? creationTarget.name : AssetDatabase.Contains(selection) ? selection.name : assetName;

            string containerPath = AssetDatabaseUtility.GetUniqueAssetPathInActiveFolder(assetName + EditorPreferences.MeshContainerAssetNamePostFix);
            string meshPath = AssetDatabaseUtility.GetUniqueAssetPathInActiveFolder(assetName + EditorPreferences.MeshAssetNamePostFix);

            var mesh = new Mesh
            {
                name = assetName
            };
            AssetDatabase.CreateAsset(mesh, meshPath);
            AssetDatabaseUtility.ForceSaveAsset(mesh, true);
            Undo.RegisterCreatedObjectUndo(mesh, "Create mesh " + assetName);
            
            var meshContainer = CreateInstance<TMeshContainer>();
            meshContainer.name = assetName;
            meshContainer.Initialize(creationTarget, mesh);
            AssetDatabase.CreateAsset(meshContainer, containerPath);
            AssetDatabaseUtility.ForceSaveAsset(meshContainer, true);
            Undo.RegisterCreatedObjectUndo(meshContainer, "Create mesh container " + assetName);

            // select the newly created Parent Asset in the Project Window
            Selection.activeObject = meshContainer;
        }
    }
}
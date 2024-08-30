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

        protected void OnDisable()
        {
            ParticleSystemAllocationFix.FlushParticleSystemAllocations();
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

            string path = AssetDatabaseUtility.GetUniqueAssetPathInActiveFolder(assetName);

            var meshContainer = CreateInstance<TMeshContainer>();
            meshContainer.name = assetName;
            var childMesh = new Mesh
            {
                name = assetName
            };
            meshContainer.Initialize(creationTarget, childMesh);
            AssetDatabase.CreateAsset(meshContainer, path);
            AssetDatabase.AddObjectToAsset(childMesh, meshContainer);
            AssetDatabaseUtility.ForceSaveAsset(meshContainer, true);
            
            Undo.RegisterCreatedObjectUndo(childMesh, "Create mesh " + assetName);
            Undo.RegisterCreatedObjectUndo(meshContainer, "Create mesh container " + assetName);

            // select the newly created Parent Asset in the Project Window
            Selection.activeObject = meshContainer;
        }
    }
}
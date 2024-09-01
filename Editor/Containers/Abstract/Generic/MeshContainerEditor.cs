using System;
using System.IO;
using JetBrains.Annotations;
using MichisVfxUtils.Editor.Utility;
using UnityEditor;
using UnityEngine;

namespace MichisVfxUtils.Editor.Containers.Abstract.Generic
{
    public abstract class MeshContainerEditor<TMeshContainer> : MeshContainerEditorBase
        where TMeshContainer : MeshContainer
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

        protected static TMeshContainer Create(string path, [CanBeNull] Action<TMeshContainer> initialize)
        {
            var assetName = Path.GetFileNameWithoutExtension(path);
            var meshContainer = CreateInstance<TMeshContainer>();
            meshContainer.name = assetName;
            var childMesh = new Mesh
            {
                name = assetName
            };

            meshContainer.Initialize(childMesh);
            initialize?.Invoke(meshContainer);
            AssetDatabase.CreateAsset(meshContainer, path);
            AssetDatabase.AddObjectToAsset(childMesh, meshContainer);
            AssetDatabaseUtility.ForceSaveAsset(meshContainer, true);

            Undo.RegisterCreatedObjectUndo(childMesh, "Create mesh " + assetName);
            Undo.RegisterCreatedObjectUndo(meshContainer, "Create mesh container " + assetName);

            // select the newly created Parent Asset in the Project Window
            Selection.activeObject = meshContainer;
            return meshContainer;
        }
    }
}
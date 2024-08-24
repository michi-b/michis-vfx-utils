using System;
using JetBrains.Annotations;
using MichisMeshMakers.Editor.Utility;
using UnityEditor;
using UnityEditor.SearchService;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MichisMeshMakers.Editor.Containers
{
    [CustomEditor(typeof(MeshContainer))]
    public abstract class MeshContainerEditor<TMeshContainer> : UnityEditor.Editor where TMeshContainer : MeshContainer
    {
        private SerializedProperty _meshProperty;

        [PublicAPI] protected TMeshContainer Target { get; private set; }

        [PublicAPI] protected TMeshContainer[] Targets { get; private set; }

        protected virtual void OnEnable()
        {
            _meshProperty = serializedObject.FindProperty(MeshContainer.MeshFieldName);
            Target = (TMeshContainer)target;
            Targets = Array.ConvertAll(targets, t => (TMeshContainer)t);
        }

        public sealed override void OnInspectorGUI()
        {
            DrawProperties();

            foreach (Object targetObject in targets)
            {
                var meshContainer = (MeshContainer)targetObject;
                Mesh mesh = meshContainer.Mesh;
                if (!string.Equals(mesh.name, meshContainer.name, StringComparison.Ordinal))
                {
                    // Delay the renaming of the mesh to after the gui update to avoid Unity logging a warning
                    // todo: Ensure that child renaming shows up instantly in the project window
                    // Currently it only shows up after changing the view there 
                    EditorApplication.delayCall += () =>
                    {
                        Undo.RecordObject(mesh, "Synchronize Mesh Container Mesh Name");
                        mesh.name = meshContainer.name;
                        EditorUtility.SetDirty(mesh);
                        AssetDatabase.SaveAssetIfDirty(mesh);
                        AssetDatabase.Refresh();
                    };
                }
            }

            foreach (TMeshContainer t in Targets)
            {
                float width = EditorGUIUtility.currentViewWidth - 35;
                Rect previewRect = GUILayoutUtility.GetRect(width, width, GUILayout.ExpandWidth(false));
                if (Event.current.type == EventType.Repaint)
                {
                    DrawMeshPreview(previewRect, t);
                }
            }
        }

        protected virtual void DrawProperties()
        {
            using (new EditorGUI.DisabledScope(true))
            {
                EditorGUILayout.PropertyField(_meshProperty, MeshContainerLabels.GeneratedMesh);
            }
        }

        protected abstract void DrawMeshPreview(Rect rect, TMeshContainer meshContainer);

        protected static void Create(string assetName, Func<string, Object> getCreationTarget)
        {
            Object selection = Selection.activeObject;
            Object creationTarget = getCreationTarget(AssetDatabase.GetAssetPath(selection));

            assetName = creationTarget != null ? creationTarget.name : AssetDatabase.Contains(selection) ? selection.name : assetName;

            string path = AssetDatabaseUtility.GetCreateAssetPath(assetName);

            var meshContainer = CreateInstance<TMeshContainer>();
            meshContainer.name = assetName;
            var childMesh = new Mesh
            {
                name = assetName
            };
            AssetDatabase.CreateAsset(meshContainer, path);
            AssetDatabase.AddObjectToAsset(childMesh, meshContainer);
            meshContainer.Initialize(creationTarget, childMesh);
            AssetDatabaseUtility.ForceSaveAsset(meshContainer);

            // select the newly created Parent Asset in the Project Window
            Selection.activeObject = meshContainer;
        }
    }
}
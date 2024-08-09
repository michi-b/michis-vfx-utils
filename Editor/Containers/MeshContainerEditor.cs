using System;
using MichisMeshMakers.Editor.Utility;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MichisMeshMakers.Editor.Containers
{
    [CustomEditor(typeof(MeshContainer))]
    public abstract class MeshContainerEditor<TMeshContainer> : UnityEditor.Editor where TMeshContainer : MeshContainer
    {
        private SerializedProperty _meshProperty;

        protected virtual void OnEnable()
        {
            _meshProperty = serializedObject.FindProperty(MeshContainer.MeshFieldName);
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
                    Undo.RecordObject(mesh, "Synchronize Mesh Container Mesh Name");
                    mesh.name = meshContainer.name;
                    EditorUtility.SetDirty(mesh);
                    AssetDatabase.SaveAssetIfDirty(mesh);
                }
            }

            float width = EditorGUIUtility.currentViewWidth - 35;
            Rect previewRect = GUILayoutUtility.GetRect(width, width, GUILayout.ExpandWidth(false));
            if (Event.current.type == EventType.Repaint)
            {
                DrawMeshPreview(previewRect);
            }
        }

        protected virtual void DrawProperties()
        {
            using (new EditorGUI.DisabledScope(true))
            {
                EditorGUILayout.PropertyField(_meshProperty, MeshContainerLabels.GeneratedMesh);
            }
        }

        protected abstract void DrawMeshPreview(Rect rect);
        
        


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
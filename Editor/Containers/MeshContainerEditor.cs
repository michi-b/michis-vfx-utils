using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MichisMeshMakers.Editor.Containers
{
    [CustomEditor(typeof(MeshContainer))]
    public abstract class MeshContainerEditor : UnityEditor.Editor
    {
        private SerializedProperty _meshProperty;

        static MeshContainerEditor()
        {
            Undo.undoRedoPerformed += OnUndoRedoPerformed;
        }

        protected virtual void OnEnable()
        {
            _meshProperty = serializedObject.FindProperty(MeshContainer.MeshFieldName);
        }

        private static void OnUndoRedoPerformed()
        {
            int affectedObjects = Undo.GetCurrentGroup();
            foreach (Object obj in Selection.objects)
            {
                if (obj is MeshContainer meshContainer)
                {
                    meshContainer.Apply();
                }
            }
        }

        public sealed override void OnInspectorGUI()
        {
            serializedObject.Update();
            
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
            
            DrawProperties();
            
            // draw mesh previews
            foreach (Object targetObject in targets)
            {
                float width = EditorGUIUtility.currentViewWidth - 35;
                Rect previewRect = GUILayoutUtility.GetRect(width, width, GUILayout.ExpandWidth(false));
                if (Event.current.type == EventType.Repaint)
                {
                    DrawMeshPreview(previewRect, targetObject);
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

        protected abstract void DrawMeshPreview(Rect previewRect, Object targetObject);
    }
}
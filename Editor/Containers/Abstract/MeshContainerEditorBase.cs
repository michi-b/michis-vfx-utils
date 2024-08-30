//#define LOG_UNDO


using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MichisMeshMakers.Editor.Containers.Abstract
{
    [CustomEditor(typeof(MeshContainer))]
    public abstract class MeshContainerEditorBase : UnityEditor.Editor
    {
        private static readonly HashSet<MeshContainer> MeshContainerChildSyncQueue = new HashSet<MeshContainer>();
        private SerializedProperty _meshProperty;

        private SerializedProperty _statisticsProperty;

        private MeshContainer[] _targetMeshContainers;
        private Mesh[] _targetMeshes;

        static MeshContainerEditorBase()
        {
            Undo.undoRedoPerformed += OnUndoRedoLegacy;
        }

        protected virtual void OnEnable()
        {
            _targetMeshContainers = new MeshContainer[targets.Length];
            for (int i = 0; i < targets.Length; i++)
            {
                _targetMeshContainers[i] = (MeshContainer)targets[i];
            }

            _meshProperty = serializedObject.FindProperty(MeshContainer.MeshFieldName);
            _statisticsProperty = serializedObject.FindProperty(MeshContainer.StatisticsFieldName);
        }

        private static void OnUndoRedoLegacy()
        {
            foreach (Object o in Selection.objects)
            {
                if (o is MeshContainer meshContainer)
                {
                    if (meshContainer != null)
                    {
                        meshContainer.Apply();
                        EditorUtility.SetDirty(meshContainer.Mesh);
                    }
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

                if (!MeshContainerChildSyncQueue.Contains(meshContainer)
                    && !string.Equals(mesh.name, meshContainer.name, StringComparison.Ordinal))
                {
                    // Delay the renaming of the mesh to after the gui update to avoid Unity logging a warning
                    // todo: Ensure that child renaming shows up instantly in the project window
                    // Currently it only shows up after changing the view there 

                    MeshContainerChildSyncQueue.Add(meshContainer);

                    EditorApplication.delayCall += () =>
                    {
                        if (MeshContainerChildSyncQueue.Contains(meshContainer))
                        {
                            Undo.RecordObject(mesh, "Synchronize Mesh Container Mesh Name");
                            mesh.name = meshContainer.name;
                            EditorUtility.SetDirty(mesh);
                            AssetDatabase.SaveAssetIfDirty(mesh);
                            AssetDatabase.Refresh();
                            Debug.Log($"Synchronized the name of mesh container {meshContainer.name} to it's child mesh.");
                            EditorGUIUtility.PingObject(mesh);
                            MeshContainerChildSyncQueue.Remove(meshContainer);
                        }
                    };
                }
            }

            DrawProperties();

            foreach (MeshContainer targetObject in _targetMeshContainers)
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

        protected abstract void DrawMeshPreview(Rect previewRect, MeshContainer targetMeshContainer);

        protected void Apply()
        {
            int undoGroup = Undo.GetCurrentGroup();

            string targetsString = string.Join(", ", targets.Select(tmc => tmc.name));
            string undoOperationName = $"Apply Container to Mesh in {targetsString}";
            Undo.RegisterCompleteObjectUndo(targets, undoOperationName);

            foreach (MeshContainer targetMeshContainer in _targetMeshContainers)
            {
                targetMeshContainer.Apply();
                EditorUtility.SetDirty(targetMeshContainer.Mesh);
            }

            Undo.CollapseUndoOperations(undoGroup);
        }
    }
}
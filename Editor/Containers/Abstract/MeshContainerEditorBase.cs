//#define LOG_UNDO

using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MichisMeshMakers.Editor.Containers.Abstract
{
    [CustomEditor(typeof(MeshContainer))]
    public abstract class MeshContainerEditorBase : UnityEditor.Editor
    {
#if UNITY_2022_1_OR_NEWER
        // stack of undo records that can be undone
        private static readonly Stack<UndoRecord> UndoStack = new Stack<UndoRecord>();

        // inverted stack of undo records that can be redone
        private static readonly Stack<UndoRecord> RedoStack = new Stack<UndoRecord>();
#endif

        private SerializedProperty _meshProperty;
        private SerializedProperty _statisticsProperty;
        private MeshContainer[] _targetMeshContainers;
        private Mesh[] _targetMeshes;

        static MeshContainerEditorBase()
        {
#if UNITY_2022_1_OR_NEWER
            Undo.undoRedoEvent += OnUndoRedo;
#else
            Undo.undoRedoPerformed += OnUndoRedoLegacy;
#endif
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

#if UNITY_2022_1_OR_NEWER
        private static void OnUndoRedo(in UndoRedoInfo undo)
        {
            int undoGroup = undo.undoGroup;

            if (!undo.isRedo) // undo
            {
                if (UndoStack.Any() && UndoStack.Peek().Group == undoGroup)
                {
                    UndoRecord undoRecord = UndoStack.Pop();
                    undoRecord.Apply();
                    RedoStack.Push(undoRecord);
                    Debug.Log($"Moved undo record {undoRecord.Group} from undo to redo stack");
                }
            }
            else // redo
            {
                if (RedoStack.Any() && RedoStack.Peek().Group == undoGroup)
                {
                    UndoRecord redoRecord = RedoStack.Pop();
                    redoRecord.Apply();
                    UndoStack.Push(redoRecord);
                    Debug.Log($"Moved undo record {redoRecord.Group} from redo to undo stack");
                }
            }
        }
#else
        private static void OnUndoRedoLegacy()
        {
            foreach (Object o in Selection.objects)
            {
                if (o is MeshContainer meshContainer)
                {
                    meshContainer.Apply();
                    EditorUtility.SetDirty(meshContainer.Mesh);
                }
            }
        }
#endif


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
#if UNITY_2022_1_OR_NEWER
            RedoStack.Clear();
#endif

            int undoGroup = Undo.GetCurrentGroup();

            string targetsString = string.Join(", ", _targetMeshContainers.Select(tmc => tmc.name));
            string undoOperationName = $"Apply Container to Mesh in {targetsString}";
            Undo.RegisterCompleteObjectUndo(_targetMeshContainers.Select(tmc => tmc.Mesh as Object).ToArray(), undoOperationName);
            foreach (MeshContainer targetMeshContainer in _targetMeshContainers)
            {
                targetMeshContainer.Apply();
            }

            Undo.CollapseUndoOperations(undoGroup);

#if UNITY_2022_1_OR_NEWER
            UndoStack.Push(new UndoRecord(undoGroup, _targetMeshContainers));
#endif

#if LOG_UNDO
            Debug.Log("Recorded Undo: " + undoGroup);
#endif
        }

#if UNITY_2022_1_OR_NEWER
        private class UndoRecord
        {
            private readonly MeshContainer[] _meshContainers;

            // ReSharper disable once ParameterTypeCanBeEnumerable.Local
            public UndoRecord(int group, MeshContainer[] meshContainers)
            {
                Group = group;
                _meshContainers = meshContainers.ToArray();
            }

            public int Group { get; }

            public void Apply()
            {
#if LOG_UNDO
                     Debug.Log($"Applying Undo: {Group}"); 
#endif
                foreach (MeshContainer meshContainer in _meshContainers)
                {
                    if (meshContainer != null)
                    {
                        meshContainer.Apply();
                        EditorUtility.SetDirty(meshContainer.Mesh);
                    }
                }
            }
        }
#endif
    }
}
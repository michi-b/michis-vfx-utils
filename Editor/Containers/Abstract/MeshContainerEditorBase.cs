//#define LOG_UNDO


using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using MichisMeshMakers.Editor.Extensions;
using MichisMeshMakers.Editor.Utility;
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
            serializedObject.UpdateIfRequiredOrScript();

            foreach (Object targetObject in targets)
            {
                var meshContainer = (MeshContainer)targetObject;
                Mesh mesh = meshContainer.Mesh;

                if (meshContainer.Mesh == null)
                {
                    var so = new SerializedObject(meshContainer);
                    SerializedProperty sp = so.FindProperty(MeshContainer.MeshFieldName);
                    sp.objectReferenceValue = AssetDatabaseUtility.LoadFromSameAsset<Mesh>(meshContainer);
                    so.ApplyModifiedProperties();
                    mesh = meshContainer.Mesh;
                    Debug.Assert(mesh != null);
                    serializedObject.Update();
                    Apply();
                }

                // handle child mesh name out of sync
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

            foreach (MeshContainer meshContainer in _targetMeshContainers)
            {
                float width = EditorGUIUtility.currentViewWidth - 35;
                Rect previewRect = GUILayoutUtility.GetRect(width, width, GUILayout.ExpandWidth(false));
                if (Event.current.type == EventType.Repaint)
                {
                    DrawMeshPreview(previewRect, meshContainer);
                }

                DrawMeshStatistics(previewRect, meshContainer);
            }
        }

        protected virtual void DrawMeshStatistics(Rect previewRect, MeshContainer meshContainer)
        {
            MeshStatistics statistics = meshContainer.Statistics;
            DrawStatistic(previewRect.TakeLineFromBottom(), MeshContainerGuiLabels.TriangleCount, (meshContainer.Mesh.GetIndexCount(0) / 3).ToString());
            DrawStatistic(previewRect.TakeLineFromBottom(), MeshContainerGuiLabels.VertexCount, meshContainer.Mesh.vertexCount.ToString());
            DrawStatistic(previewRect.TakeLineFromBottom(), MeshContainerGuiLabels.SurfaceArea, FormatSurfaceArea(statistics.SurfaceArea));
        }

        protected virtual string FormatSurfaceArea(float surfaceArea)
        {
            return surfaceArea.AsPercentage();
        }


        [PublicAPI]
        protected static void DrawStatistic(Rect rect, GUIContent label, string value)
        {
            EditorGUI.LabelField(rect.TakeFromLeftRelative(0.5f), label);
            EditorGUI.LabelField(rect, value, GuiStyles.RightAlignedLabel);
        }

        protected virtual void DrawProperties()
        {
            using (new EditorGUI.DisabledScope(true))
            {
                EditorGUILayout.PropertyField(_meshProperty, MeshContainerGuiLabels.GeneratedMesh);
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

        protected static string GetCreateAssetPathFromTarget(Object target)
        {
            string texturePath = AssetDatabase.GetAssetPath(target);
            string assetPath = Path.ChangeExtension(texturePath, "asset");
            return AssetDatabase.GenerateUniqueAssetPath(assetPath);
        }
    }
}
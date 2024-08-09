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

        public override void OnInspectorGUI()
        {
            using (new EditorGUI.DisabledScope(true))
            {
                EditorGUILayout.PropertyField(_meshProperty, MeshContainerLabels.GeneratedMesh);
            }

            // foreach (Object targetObject in targets)
            // {
            //     var meshContainer = (MeshContainer)targetObject;
            //     if (meshContainer.Mesh == null)
            //     {
            //         Undo.RecordObject(meshContainer, "Assign missing mesh container mesh");
            //         var mesh = AssetDatabase.LoadAssetAtPath<Mesh>(AssetDatabase.GetAssetPath(meshContainer));
            //         meshContainer.Mesh = mesh;
            //         AssetDatabaseUtility.ForceSaveAsset(meshContainer);
            //     }
            // }

            // base.OnInspectorGUI();
        }


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
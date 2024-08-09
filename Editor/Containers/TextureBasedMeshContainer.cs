using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MichisMeshMakers.Editor.Containers
{
    public abstract class TextureBasedMeshContainer : MeshContainer
    {
        public const string TextureFieldName = nameof(_texture);
        
        [SerializeField] private Texture2D _texture;

        protected override void Initialize(Object selection)
        {
            _texture = selection as Texture2D;
            if(_texture == null)
            {
                Mesh mesh = Mesh;
                // todo: check if empty mesh initialization is necessary?
                mesh.vertices = Array.Empty<Vector3>();
                mesh.triangles = Array.Empty<int>();
                mesh.RecalculateNormals();
                mesh.RecalculateBounds();
            }
            Initialize();
        }
        
        protected abstract void Initialize();
        
    }
}
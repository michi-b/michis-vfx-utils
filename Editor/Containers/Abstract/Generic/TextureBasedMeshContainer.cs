using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MichisMeshMakers.Editor.Containers.Abstract.Generic
{
    public abstract class TextureBasedMeshContainer : MeshContainer
    {
        public const string TextureFieldName = nameof(_texture);

        // maximum extent of vertices (positive and negative)
        protected const float Limit = 0.5f;
        
        [SerializeField] private Texture2D _texture;

        public Texture2D Texture => _texture;

        protected override void Initialize(Object selection)
        {
            _texture = selection as Texture2D;
            if (_texture == null)
            {
                Mesh mesh = Mesh;
                // todo: check if empty mesh initialization is necessary?
                mesh.vertices = Array.Empty<Vector3>();
                mesh.triangles = Array.Empty<int>();
                mesh.RecalculateNormals();
                mesh.RecalculateBounds();
            }
        }
    }
}
using UnityEngine;

namespace Michis.VfxUtils.Editor.Containers.Abstract
{
    public abstract class MeshContainer : ScriptableObject
    {
        public const string MeshFieldName = nameof(_mesh);

        [SerializeField] private Mesh _mesh;
        [SerializeField] private MeshStatistics _statistics;

        public Mesh Mesh => _mesh;
        public MeshStatistics Statistics => _statistics;

        public void Initialize(Mesh childMesh)
        {
            _mesh = childMesh;
            Apply();
        }

        public void Apply()
        {
            Regenerate();
            _statistics = new MeshStatistics(_mesh);
        }

        protected abstract void Regenerate();
    }
}
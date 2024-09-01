using UnityEngine;
using Object = UnityEngine.Object;

namespace MichisMeshMakers.Editor.Containers.Abstract
{
    public abstract class MeshContainer : ScriptableObject
    {
        public const string MeshFieldName = nameof(_mesh);
        public const string StatisticsFieldName = nameof(_statistics);

        [SerializeField] private Mesh _mesh;
        [SerializeField] private MeshStatistics _statistics;

        public Mesh Mesh => _mesh;
        public MeshStatistics Statistics => _statistics;

        public void Initialize(Object selection, Mesh childMesh)
        {
            _mesh = childMesh;
            Initialize(selection);
            Apply();
        }

        protected virtual void Initialize(Object selection)
        {
        }

        public void Apply()
        {
            Regenerate();
            _statistics = new MeshStatistics(_mesh);
        }

        protected abstract void Regenerate();
    }
}
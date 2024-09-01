namespace MichisMeshMakers.Editor
{
    public static class Menu
    {
        public const string Name = "Michi's Mesh Makers";
        private const string Path = Name + "/";
        public const string CreateAssetPath = "Assets/Create/" + Path;
        public const string WindowPath = "Window/" + Path;
        public const string Tools = "Tools/" + Path;

        public static class Context
        {
            private const string ContextPrefix = "CONTEXT/";
            public const string ParticleSystem = ContextPrefix + nameof(ParticleSystem) + "/" + Path;
        }
    }
}
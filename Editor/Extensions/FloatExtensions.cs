namespace MichisUnityVfxUtilities.MichisUnityVfxUtilities.Editor.Extensions
{
    public static class FloatExtensions
    {
        public static string AsPercentage(this float value)
        {
            return $"{value:P2}";
        }
    }
}
using UnityEngine;

namespace MichisMeshMakers.Editor.Extensions
{
    public static class Vector3Extensions
    {
        public static Vector2 GetXy(this Vector3 vector)
        {
            return new Vector2(vector.x, vector.y);
        }
    }
}
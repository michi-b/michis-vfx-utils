using System;
using UnityEngine;

namespace MichisUnityVfxUtilities.MichisUnityVfxUtilities.Editor.Extensions
{
    public static class Ray2DExtensions
    {
        public static Vector2 Intersect(this Ray2D target, Ray2D other)
        {
            var p1 = target.origin;
            var d1 = target.direction;
            var p2 = other.origin;
            var d2 = other.direction;

            // Calculate the determinant
            var determinant = d1.x * d2.y - d1.y * d2.x;

            // If determinant is zero, the rays are parallel and do not intersect
            if (Mathf.Abs(determinant) < Mathf.Epsilon)
                throw new InvalidOperationException($"Rays {target} and {other} are parallel and do not intersect.");

            // Calculate the intersection point using Cramer's rule
            var t1 = ((p2.x - p1.x) * d2.y - (p2.y - p1.y) * d2.x) / determinant;
            return p1 + t1 * d1;
        }
    }
}
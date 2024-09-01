using System;
using UnityEngine;

namespace MichisMeshMakers.Editor.Extensions
{
    public static class Ray2DExtensions
    {
        public static Vector2 Intersect(this Ray2D target, Ray2D other)
        {
            Vector2 p1 = target.origin;
            Vector2 d1 = target.direction;
            Vector2 p2 = other.origin;
            Vector2 d2 = other.direction;

            // Calculate the determinant
            float determinant = d1.x * d2.y - d1.y * d2.x;

            // If determinant is zero, the rays are parallel and do not intersect
            if (Mathf.Abs(determinant) < Mathf.Epsilon)
            {
                throw new InvalidOperationException($"Rays {target} and {other} are parallel and do not intersect.");
            }

            // Calculate the intersection point using Cramer's rule
            float t1 = ((p2.x - p1.x) * d2.y - (p2.y - p1.y) * d2.x) / determinant;
            return p1 + t1 * d1;
        }
    }
}
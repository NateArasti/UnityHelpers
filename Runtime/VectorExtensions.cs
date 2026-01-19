using UnityEngine;

namespace HelperFunctions
{
    public static class VectorExtensions
    {
        /// <summary>
        /// Returns new Vector2 with x as x and z as y
        /// </summary>
        public static Vector2 ToXZVector2(this Vector3 vector)
        {
            return new Vector2(vector.x, vector.z);
        }

        /// <summary>
        /// Returns new Vector2 with y as x and z as y
        /// </summary>
        public static Vector2 ToYZVector2(this Vector3 vector)
        {
            return new Vector2(vector.y, vector.z);
        }

        /// <summary>
        /// Return copy of a vector with only x and y coordinates
        /// </summary>
        public static Vector3 GetXY(this Vector3 vector, float zOverride = 0)
        {
            return new Vector3(vector.x, vector.y, zOverride);
        }

        /// <summary>
        /// Return copy of a vector with only x and z coordinates
        /// </summary>
        public static Vector3 GetXZ(this Vector3 vector, float yOverride)
        {
            return new Vector3(vector.x, yOverride, vector.z);
        }

        /// <summary>
        /// Return copy of a vector with only y and z coordinates
        /// </summary>
        public static Vector3 GetYZ(this Vector3 vector, float xOverride)
        {
            return new Vector3(xOverride, vector.y, vector.z);
        }
        
        /// <summary>
        /// Set X component
        /// </summary>
        public static Vector3 WithX(this Vector3 vector, float x)
        {
            return new Vector3(x, vector.y, vector.z);
        }

        /// <summary>
        /// Set Y component
        /// </summary>
        public static Vector3 WithY(this Vector3 vector, float y)
        {
            return new Vector3(vector.x, y, vector.z);
        }

        /// <summary>
        /// Set Z component
        /// </summary>
        public static Vector3 WithZ(this Vector3 vector, float z)
        {
            return new Vector3(vector.x, vector.y, z);
        }

        /// <summary>
        /// Set X component of Vector2
        /// </summary>
        public static Vector2 WithX(this Vector2 vector, float x)
        {
            return new Vector2(x, vector.y);
        }

        /// <summary>
        /// Set Y component of Vector2
        /// </summary>
        public static Vector2 WithY(this Vector2 vector, float y)
        {
            return new Vector2(vector.x, y);
        }

        /// <summary>
        /// Add to X component
        /// </summary>
        public static Vector3 AddX(this Vector3 vector, float x)
        {
            return new Vector3(vector.x + x, vector.y, vector.z);
        }

        /// <summary>
        /// Add to Y component
        /// </summary>
        public static Vector3 AddY(this Vector3 vector, float y)
        {
            return new Vector3(vector.x, vector.y + y, vector.z);
        }

        /// <summary>
        /// Add to Z component
        /// </summary>
        public static Vector3 AddZ(this Vector3 vector, float z)
        {
            return new Vector3(vector.x, vector.y, vector.z + z);
        }

        /// <summary>
        /// Convert Vector2 to Vector3 with x as x, y as z
        /// </summary>
        public static Vector3 ToXZVector3(this Vector2 vector, float y = 0)
        {
            return new Vector3(vector.x, y, vector.y);
        }

        /// <summary>
        /// Convert Vector2 to Vector3 with x as x, y as y
        /// </summary>
        public static Vector3 ToXYVector3(this Vector2 vector, float z = 0)
        {
            return new Vector3(vector.x, vector.y, z);
        }

        /// <summary>
        /// Clamp vector between min and max
        /// </summary>
        public static Vector3 Clamp(this Vector3 vector, Vector3 min, Vector3 max)
        {
            return new Vector3(
                Mathf.Clamp(vector.x, min.x, max.x),
                Mathf.Clamp(vector.y, min.y, max.y),
                Mathf.Clamp(vector.z, min.z, max.z)
            );
        }

        /// <summary>
        /// Clamp vector between min and max
        /// </summary>
        public static Vector2 Clamp(this Vector2 vector, Vector2 min, Vector2 max)
        {
            return new Vector2(
                Mathf.Clamp(vector.x, min.x, max.x),
                Mathf.Clamp(vector.y, min.y, max.y)
            );
        }

        /// <summary>
        /// Get absolute value of all components
        /// </summary>
        public static Vector3 Abs(this Vector3 vector)
        {
            return new Vector3(Mathf.Abs(vector.x), Mathf.Abs(vector.y), Mathf.Abs(vector.z));
        }

        /// <summary>
        /// Get absolute value of all components
        /// </summary>
        public static Vector2 Abs(this Vector2 vector)
        {
            return new Vector2(Mathf.Abs(vector.x), Mathf.Abs(vector.y));
        }

        /// <summary>
        /// Multiply component-wise
        /// </summary>
        public static Vector3 Multiply(this Vector3 a, Vector3 b)
        {
            return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
        }

        /// <summary>
        /// Divide component-wise
        /// </summary>
        public static Vector3 Divide(this Vector3 a, Vector3 b)
        {
            return new Vector3(a.x / b.x, a.y / b.y, a.z / b.z);
        }

        /// <summary>
        /// Get direction to another vector
        /// </summary>
        public static Vector3 DirectionTo(this Vector3 from, Vector3 to)
        {
            return (to - from).normalized;
        }

        /// <summary>
        /// Get closest point on line segment
        /// </summary>
        public static Vector3 ClosestPointOnLine(this Vector3 point, Vector3 lineStart, Vector3 lineEnd)
        {
            var line = lineEnd - lineStart;
            var t = Mathf.Clamp01(Vector3.Dot(point - lineStart, line) / line.sqrMagnitude);
            return lineStart + line * t;
        }

        /// <summary>
        /// Check if approximately equal
        /// </summary>
        public static bool Approximately(this Vector3 a, Vector3 b, float tolerance = 0.0001f)
        {
            return Vector3.SqrMagnitude(a - b) < tolerance * tolerance;
        }
    }
}

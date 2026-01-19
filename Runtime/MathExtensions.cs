using UnityEngine;

namespace HelperFunctions
{
    public static class MathExtensions
    {
        public const float TOLERANCE = 0.001f;

        /// <summary>
        /// Compares if two values are approximately equal within provided tolerance
        /// </summary>
        public static bool ApproximatelyEqual(float a, float b, float tolerance = TOLERANCE)
        {
            return Mathf.Abs(a - b) < tolerance;
        }

        /// <summary>
        /// Round a value to nearest int value determined by stepValue.
        /// So if stepValue is 5, we round 11 to 10 because we want to go in steps of 5 such as 0, 5, 10, 15.
        /// </summary>
        public static int RoundValueToStep(float value, float stepValue)
        {
            if (stepValue <= 0)
            {
                throw new UnityException($"List size should be positive  - {stepValue}");
            }

            return (int)(Mathf.Round(value / stepValue) * stepValue);
        }

        /// <summary>
        /// Adds value to average
        /// </summary>
        public static float AddValueToAverage(float currentAverage, float valueToAdd, float count)
        {
            return (currentAverage * count + valueToAdd) / (count + 1f);
        }

        /// <summary>
        /// Remaps from first range to second range
        /// </summary>
        public static float Remap(
            float value,
            float firstRangeStart, float firstRangeEnd,
            float secondRangeStart, float secondRangeEnd
            )
        {
            return secondRangeStart + (value - firstRangeStart) *
                ((secondRangeEnd - secondRangeStart) / (firstRangeEnd - firstRangeStart));
        }

        /// <summary>
        /// Remaps from first range to second range
        /// </summary>
        public static float Remap(
            float value,
            Vector2 firstRange,
            Vector2 secondRange
            )
        {
            return Remap(
                value,
                firstRange.x,
                firstRange.y,
                secondRange.x,
                secondRange.y
            );
        }

        /// <summary>
        /// Remap and clamp value to new range
        /// </summary>
        public static float RemapClamped(float value, float fromMin, float fromMax, float toMin, float toMax)
        {
            return Mathf.Clamp(
                Remap(value, fromMin, fromMax, toMin, toMax),
                Mathf.Min(toMin, toMax),
                Mathf.Max(toMin, toMax)
            );
        }

        /// <summary>
        /// Wrap value between min and max (like modulo but works with floats and negative values)
        /// </summary>
        public static float Wrap(float value, float min, float max)
        {
            var range = max - min;
            return value - range * Mathf.Floor((value - min) / range);
        }

        /// <summary>
        /// Exponential decay lerp (smooth following)
        /// </summary>
        public static float ExpDecay(float current, float target, float decay, float deltaTime)
        {
            return target + (current - target) * Mathf.Exp(-decay * deltaTime);
        }

        /// <summary>
        /// Exponential decay lerp for Vector3
        /// </summary>
        public static Vector3 ExpDecay(Vector3 current, Vector3 target, float decay, float deltaTime)
        {
            return target + (current - target) * Mathf.Exp(-decay * deltaTime);
        }

        /// <summary>
        /// Map value to 0-1 range (same as InverseLerp but clearer name)
        /// </summary>
        public static float Normalize(float value, float min, float max)
        {
            return (value - min) / (max - min);
        }

        /// <summary>
        /// Get percentage between two values
        /// </summary>
        public static float GetPercentage(float current, float min, float max)
        {
            return Normalize(current, min, max) * 100f;
        }

        /// <summary>
        /// Check if value is in range (inclusive)
        /// </summary>
        public static bool InRange(float value, float min, float max)
        {
            return value >= min && value <= max;
        }

        /// <summary>
        /// Check if value is in range (exclusive)
        /// </summary>
        public static bool InRangeExclusive(float value, float min, float max)
        {
            return value > min && value < max;
        }

        /// <summary>
        /// Snap Vector3 to grid
        /// </summary>
        public static Vector3 SnapToGrid(Vector3 position, float gridSize)
        {
            return new Vector3(
                Mathf.Round(position.x / gridSize) * gridSize,
                Mathf.Round(position.y / gridSize) * gridSize,
                Mathf.Round(position.z / gridSize) * gridSize
            );
        }

        /// <summary>
        /// Average of multiple values
        /// </summary>
        public static float Average(params float[] values)
        {
            if (values.Length == 0)
            {
                return 0f;
            }

            var sum = 0f;
            foreach (var v in values)
            {
                sum += v;
            }

            return sum / values.Length;
        }
    
        /// <summary>
        /// Calculates flat index for 3-dimensional array
        /// </summary>
        public static long GetFlatIndex(long x, long y, long xMax)
        {
            return x + xMax * y;
        }

        /// <summary>
        /// Calculates flat index for 3-dimensional array
        /// </summary>
        public static long GetFlatIndex(long x, long y, long z, long xMax, long yMax)
        {
            return x + xMax * y + yMax * xMax * z;
        }
    }
}

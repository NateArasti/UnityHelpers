using UnityEngine;

namespace HelperFunctions
{
    public static class ColorExtensions
    {
        /// <summary>
        /// Set alpha channel
        /// </summary>
        public static Color WithAlpha(this Color color, float alpha)
        {
            return new Color(color.r, color.g, color.b, alpha);
        }

        /// <summary>
        /// Set red channel
        /// </summary>
        public static Color WithRed(this Color color, float red)
        {
            return new Color(red, color.g, color.b, color.a);
        }

        /// <summary>
        /// Set green channel
        /// </summary>
        public static Color WithGreen(this Color color, float green)
        {
            return new Color(color.r, green, color.b, color.a);
        }

        /// <summary>
        /// Set blue channel
        /// </summary>
        public static Color WithBlue(this Color color, float blue)
        {
            return new Color(color.r, color.g, blue, color.a);
        }

        /// <summary>
        /// Lighten color by amount (0-1)
        /// </summary>
        public static Color Lighten(this Color color, float amount)
        {
            return Color.Lerp(color, Color.white, Mathf.Clamp01(amount)).WithAlpha(color.a);
        }

        /// <summary>
        /// Darken color by amount (0-1)
        /// </summary>
        public static Color Darken(this Color color, float amount)
        {
            return Color.Lerp(color, Color.black, Mathf.Clamp01(amount)).WithAlpha(color.a);
        }

        /// <summary>
        /// Increase saturation (using conversion to HSV)
        /// </summary>
        public static Color Saturate(this Color color, float amount)
        {
            Color.RGBToHSV(color, out float h, out float s, out float v);
            return Color.HSVToRGB(h, Mathf.Clamp01(s + amount), v).WithAlpha(color.a);
        }

        /// <summary>
        /// Invert RGB channels
        /// </summary>
        public static Color Invert(this Color color)
        {
            return new Color(1f - color.r, 1f - color.g, 1f - color.b, color.a);
        }

        /// <summary>
        /// Convert to grayscale using luminance
        /// </summary>
        public static Color ToGrayscale(this Color color)
        {
            float gray = color.r * 0.299f + color.g * 0.587f + color.b * 0.114f;
            return new Color(gray, gray, gray, color.a);
        }

        /// <summary>
        /// Get contrasting color (black or white)
        /// </summary>
        public static Color GetContrastingColor(this Color color)
        {
            float luminance = color.r * 0.299f + color.g * 0.587f + color.b * 0.114f;
            return luminance > 0.5f ? Color.black : Color.white;
        }

        /// <summary>
        /// Convert to hex string
        /// </summary>
        public static string ToHex(this Color color, bool includeAlpha = false)
        {
            if (includeAlpha)
                return $"#{ColorUtility.ToHtmlStringRGBA(color)}";
            return $"#{ColorUtility.ToHtmlStringRGB(color)}";
        }

        /// <summary>
        /// Multiply RGB channels by value (ignoring alpha)
        /// </summary>
        public static Color MultiplyRGB(this Color color, float multiplier)
        {
            return new Color(
                color.r * multiplier,
                color.g * multiplier,
                color.b * multiplier,
                color.a
            );
        }
    }
}

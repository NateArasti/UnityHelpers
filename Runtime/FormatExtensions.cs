using System.Globalization;
using UnityEngine;

namespace HelperFunctions
{
    public static class FormatExtensions
    {
        /// <summary>
        /// Formats bytes to KB, MB or GB
        /// </summary>
        public static string FormatBytes(long bytes)
        {
            if (bytes < 0)
            {
                bytes = 0;
            }

            const float KB = 1024f;
            const float MB = KB * 1024f;
            const float GB = MB * 1024f;

            if (bytes < KB)
            {
                return $"{bytes} B";
            }
            if (bytes < MB)
            {
                return (bytes / KB).ToString("0.##", CultureInfo.InvariantCulture) + " KB";
            }
            if (bytes < GB)
            {
                return (bytes / MB).ToString("0.##", CultureInfo.InvariantCulture) + " MB";
            }

            return (bytes / GB).ToString("0.##", CultureInfo.InvariantCulture) + " GB";
        }
        
        /// <summary>
        /// Formats seconds to hh:mm:ss
        /// </summary>
        public static string FormatSeconds(float seconds)
        {
            if (seconds < 0f)
            {
                seconds = 0f;
            }

            var totalSeconds = Mathf.FloorToInt(seconds);

            var hours = totalSeconds / 3600;
            var mins  = (totalSeconds % 3600) / 60;
            var secs  = totalSeconds % 60;

            return hours > 0
                ? $"{hours:00}:{mins:00}:{secs:00}"
                : $"{mins:00}:{secs:00}";
        }
    }
}

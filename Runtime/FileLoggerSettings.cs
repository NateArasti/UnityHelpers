#if UNITY_EDITOR
using UnityEditor;
#endif
using System.IO;
using UnityEngine;
using System.Collections.Generic;

namespace ExtendedFileLogger
{
    public class FileLoggerSettings : ScriptableObject
    {
        public enum LogPath
        {
            PersistentDataPath,
            DataPath,
        }

        public const string SETTINGS_FILE_NAME = "LoggerSettings.asset";
        public const string SETTINGS_PATH = "Assets/Resources/";

        public bool IncludeStackTrace = false;
        public List<PlatformSettings> PlatformsSettings = new()
        {
            new(RuntimePlatform.IPhonePlayer, false),
            new(RuntimePlatform.Android, false),
            new(RuntimePlatform.LinuxServer, false),
            new(RuntimePlatform.WindowsServer, false),
        };

        public static FileLoggerSettings GetOrCreateSettings()
        {
#if UNITY_EDITOR
            if (!Directory.Exists(SETTINGS_PATH))
            {
                AssetDatabase.CreateFolder("Assets", "Resources");
            }
#endif
            var settings = Resources.Load<FileLoggerSettings>(Path.GetFileNameWithoutExtension(SETTINGS_FILE_NAME));
            if (settings == null)
            {
                Debug.Log($"No {nameof(FileLoggerSettings)}, creating");
                settings = ScriptableObject.CreateInstance<FileLoggerSettings>();
#if UNITY_EDITOR
                var path = Path.Combine(SETTINGS_PATH, SETTINGS_FILE_NAME);
                AssetDatabase.CreateAsset(settings, path);
                AssetDatabase.SaveAssets();
#endif
            }
            return settings;
        }

        [System.Serializable]
        public class PlatformSettings
        {
            public RuntimePlatform Platform;
            public bool Enabled;
            public LogPath LogPath = LogPath.PersistentDataPath;

            public PlatformSettings(RuntimePlatform platform, bool enabled)
            {
                Platform = platform;
                Enabled = enabled;
            }
        }
    }
}

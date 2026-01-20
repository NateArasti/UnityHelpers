using System;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Scripting;

[assembly: AlwaysLinkAssembly]

namespace ExtendedFileLogger
{
    public static class FileLogger
    {
        private const int MAX_TEMP_LOG_LENGTH = 100;
        private const string LOG_FILE_NAME = "PlayerLogs.txt";
        private const string LOG_FILE_NAME_OLD = "PlayerLogs_old.txt";
        private const string SEPARATOR = "----------";

        private static readonly StringBuilder s_CurrentLogs = new();
        private static FileLoggerSettings s_Settings;
        private static FileLoggerSettings.PlatformSettings s_PlatformSettings;

        private static string CurrentTime => System.DateTime.Now.ToString("HH-mm-ss");
        private static string SavePath => GetLogPath(s_PlatformSettings.LogPath, LOG_FILE_NAME);
        private static string OldSavePath => GetLogPath(s_PlatformSettings.LogPath, LOG_FILE_NAME_OLD);

        private static string GetLogPath(FileLoggerSettings.LogPath logPath, string logName)
        {
            var directoryPath = logPath switch
            {
                FileLoggerSettings.LogPath.DataPath => Application.dataPath,
                FileLoggerSettings.LogPath.PersistentDataPath => Application.persistentDataPath,
                _ => throw new System.NotImplementedException(),
            };
            return Path.Combine(directoryPath, logName);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        public static void Init()
        {
            s_Settings = FileLoggerSettings.GetOrCreateSettings();

            if (s_Settings == null)
            {
                Debug.LogError($"No settings for {nameof(FileLogger)}");
                return;
            }

            foreach (var platformSettings in s_Settings.PlatformsSettings)
            {
                if (platformSettings.Platform == Application.platform)
                {
                    s_PlatformSettings = platformSettings;
                }
            }

            if (s_PlatformSettings == null || !s_PlatformSettings.Enabled)
            {
                Debug.Log($"Extended File Logging for {Application.platform} is not enabled");
                return;
            }

            if (File.Exists(SavePath))
            {
                if (File.Exists(OldSavePath))
                {
                    File.Delete(OldSavePath);
                }
                File.Copy(SavePath, OldSavePath);
                File.Delete(SavePath);
            }
            File.Create(SavePath).Dispose();

            s_CurrentLogs.Clear();
            Application.logMessageReceived += HandleLog;
            Application.quitting += WriteLogs;
            Application.focusChanged += OnFocusChanged;
            Debug.Log($"[{nameof(FileLogger)}]: {DateTime.Today}");
            Debug.Log($"[{nameof(FileLogger)}]: Initialized");
            Debug.Log($"[{nameof(FileLogger)}]: Logs will be saved at {SavePath}");
        }

        private static void OnFocusChanged(bool focus) // for android devices
        {
            if(!focus)
            {
                WriteLogs();
            }
        }

        private static void WriteLogs()
        {
            if (s_CurrentLogs.Length > 0)
            {
                File.AppendAllText(SavePath, s_CurrentLogs.ToString());
                s_CurrentLogs.Clear();
            }
        }

        private static void HandleLog(string condition, string stackTrace, LogType type)
        {
            var logLine = $"[{type}, Time: {CurrentTime}]: {condition}";
            s_CurrentLogs.AppendLine(logLine);
            if(s_Settings.IncludeStackTrace)
            {
                s_CurrentLogs.AppendLine(stackTrace);
            }

            s_CurrentLogs.AppendLine(SEPARATOR);

            if (s_CurrentLogs.Length > MAX_TEMP_LOG_LENGTH)
            {
                WriteLogs();
            }
        }
    }
}

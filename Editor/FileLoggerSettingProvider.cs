using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ExtendedFileLogger.Editor
{
    public static class FileLoggerSettingProvider
    {
        private static FileLoggerSettings s_Settings;

        [SettingsProvider]
        public static SettingsProvider CreateMyCustomSettingsProvider()
        {
            s_Settings = FileLoggerSettings.GetOrCreateSettings();

            var provider = new SettingsProvider("Project/FileLoggerSettings", SettingsScope.Project)
            {
                label = "File Logger Settings",
                guiHandler = SettingsGUIDraw,
                keywords = new HashSet<string>(new[] { "Logger", "File", "Platform", "Enabled", "Include Stack Trace" })
            };

            return provider;
        }

        private static void SettingsGUIDraw(string context)
        {
            var hasChange = false;

            EditorGUILayout.BeginVertical();

            var includeStackTrace = EditorGUILayout.Toggle("Include Stack Trace", s_Settings.IncludeStackTrace);

            if (includeStackTrace != s_Settings.IncludeStackTrace)
            {
                s_Settings.IncludeStackTrace = includeStackTrace;
                hasChange = true;
            }
            if (s_Settings.IncludeStackTrace && !EditorUserBuildSettings.development)
            {
                EditorGUILayout.BeginVertical();
                EditorGUILayout.HelpBox("For full stack trace you should enable development build", MessageType.Warning);
                if (GUILayout.Button("Enable"))
                {
                    EditorUserBuildSettings.development = true;
                }
                EditorGUILayout.EndVertical();
            }
            GUILayout.Space(20);

            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical("Platforms Settings", "window");
            GUILayout.Space(10);
            foreach (var platformSettings in s_Settings.PlatformsSettings)
            {
                GUILayout.BeginHorizontal();
                var enabled = EditorGUILayout.Toggle(platformSettings.Platform.ToString(), platformSettings.Enabled);
                if (enabled != platformSettings.Enabled)
                {
                    platformSettings.Enabled = enabled;
                    hasChange = true;
                }
                if (enabled)
                {
                    var newPath = (FileLoggerSettings.LogPath)EditorGUILayout.EnumPopup(platformSettings.LogPath);
                    if (newPath != platformSettings.LogPath)
                    {
                        platformSettings.LogPath = newPath;
                        hasChange = true;
                    }
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.Space(10);
            GUILayout.EndVertical();
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.FlexibleSpace();

            EditorGUILayout.EndVertical();

            if (hasChange)
            {
                EditorUtility.SetDirty(s_Settings);
            }
        }
    }
}

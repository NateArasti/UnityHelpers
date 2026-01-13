using UnityEditor;
using UnityEngine;

namespace UNotes
{
    /// <summary>
    /// Class to show window for handling all notes
    /// </summary>
    public class UNotesWindow : EditorWindow
    {
        /// <summary>
        /// Type of notes window can handle
        /// </summary>
        public enum NotesType
        {
            Project,
            Objects
        }

        private const string ProjectTitleText = "Here you can see all project notes.";
        private const string ObjectTitleText = "Here you can see all notes that were put on some unity object/entity.\nClick ping to view which object's note you're seeing.";
        private const string NoNotesText = "Don't see any Notes on current objects! To add them, click + button in inspector header.";

        /// <summary>
        /// Current instance of a window
        /// </summary>
        public static UNotesWindow Instance { get; private set; }
        /// <summary>
        /// Check if window is exists/opened
        /// </summary>
        public static bool Exists => Instance != null;
        /// <summary>
        /// Current type window is handling
        /// </summary>
        public static NotesType CurrentNotesType { get; set; }

        private UNote m_CurrentEditNote;
        private Vector2 m_ScrollPosition;

        [MenuItem("Window/UNotes")]
        public static void ShowWindow()
        {
            Instance = GetWindow<UNotesWindow>("UNotes");

            Instance.titleContent = new GUIContent(Instance.titleContent.text, UNoteUtility.s_NotesIcon.Texture);

            Instance.Show();
        }

        private void OnEnable()
        {
            if (!Exists)
            {
                Instance = this;
            }
        }

        private void OnGUI()
        {
            var inEdit = false;

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            if (CurrentNotesType == NotesType.Project)
            {
                GUI.enabled = false;
            }
            if (GUILayout.Button("Project Notes", EditorStyles.toolbarButton))
            {
                CurrentNotesType = NotesType.Project;
            }
            GUI.enabled = true;

            if (CurrentNotesType == NotesType.Objects)
            {
                GUI.enabled = false;
            }
            if (GUILayout.Button("Object Notes", EditorStyles.toolbarButton))
            {
                CurrentNotesType = NotesType.Objects;
            }
            GUI.enabled = true;

            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
            
            var header = CurrentNotesType switch
            {
                NotesType.Project => ProjectTitleText,
                NotesType.Objects => ObjectTitleText,
                _ => throw new System.NotImplementedException(),
            };
            EditorGUILayout.Space();
            GUILayout.Label(header, EditorStyles.centeredGreyMiniLabel);
            EditorGUILayout.Space();

            m_ScrollPosition = EditorGUILayout.BeginScrollView(m_ScrollPosition);

            if (UNotesStorage.Instance != null)
            {
                var notesList = CurrentNotesType switch
                {
                    NotesType.Project => UNotesStorage.Instance.ProjectNotes,
                    NotesType.Objects => UNotesStorage.Instance.ObjectNotes,
                    _ => throw new System.NotImplementedException(),
                };
                if (notesList.Count > 0)
                {
                    for (var i = 0; i < notesList.Count; i++)
                    {
                        var note = notesList[i];
                        var isEditing = m_CurrentEditNote == note;
                        UNoteUtility.DrawNoteInlineGUI(note, position.width, ref isEditing);

                        inEdit = inEdit || isEditing;
                        if (isEditing)
                        {
                            m_CurrentEditNote = note;
                        }
                        else if (note == m_CurrentEditNote)
                        {
                            m_CurrentEditNote = null;
                        }

                        EditorGUILayout.Space();
                    }
                }
                else
                {
                    GUILayout.FlexibleSpace();
                    GUILayout.Label(NoNotesText, EditorStyles.centeredGreyMiniLabel);
                    GUILayout.FlexibleSpace();
                }
            }

            if (CurrentNotesType == NotesType.Project)
            {
                var addedNewNote = false;
                UNoteUtility.DrawAddNoteGUI(ref addedNewNote);
                if (addedNewNote)
                {
                    m_CurrentEditNote = UNotesStorage.Instance.ProjectNotes[^1];
                }
            }

            EditorGUILayout.Space();
            EditorGUILayout.EndScrollView();

            var cachedColor = GUI.color;
            GUI.color = Color.red;
            if (GUILayout.Button("Clear all notes"))
            {
                if (EditorUtility.DisplayDialog(
                    "Delete all notes",
                    "Are you sure you want to delete all notes?",
                    "Yes",
                    "No"))
                {
                    UNotesStorage.Instance.ClearAll();
                }
            }
            GUI.color = cachedColor;

            if (!inEdit)
            {
                UNoteUtility.ResetEdit();
            }
        }
    }
}

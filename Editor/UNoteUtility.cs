using UnityEditor;
using UnityEngine;

namespace UNotes
{
    /// <summary>
    /// Utility class for handling most of common low-level operations
    /// </summary>
    public static class UNoteUtility
    {
        private const string k_NotePlaceholderText = "<i>Enter note text</i>";

        #region Icons

        internal readonly static BuiltInEditorIcon s_NotesIcon = new ("d_FilterByLabel");
        internal readonly static BuiltInEditorIcon s_EndEditIcon = new ("d_FilterSelectedOnly@2x");
        internal readonly static BuiltInEditorIcon s_EditIcon = new ("d_editicon.sml");
        internal readonly static BuiltInEditorIcon s_RemoveIcon = new ("Grid.EraserTool");
        internal readonly static BuiltInEditorIcon s_AddIcon = new ("d_Toolbar Plus@2x");
        internal readonly static BuiltInEditorIcon s_PingIcon = new ("animationvisibilitytoggleon");
        internal readonly static BuiltInEditorIcon s_CopyIcon = new("d_UnityEditor.ConsoleWindow");

        #endregion

        #region Styles
        
        private static GUIStyle s_RichLargeLabel;

        #endregion

        #region GUI

        private static GUIContent s_EndEditContent;
        private static GUIContent s_EditContent;
        private static GUIContent s_RemoveEditContent;
        private static GUIContent s_AddContent;
        private static GUIContent s_CopyContent;
        private static GUIContent s_PingContent;

        #endregion

        private static string s_EditingText;

        private static void Initialize()
        {
            s_RichLargeLabel = new GUIStyle(EditorStyles.largeLabel);
            s_RichLargeLabel.richText = true;

            s_EditContent = new GUIContent(s_EditIcon.Texture, "Edit note");
            s_RemoveEditContent = new GUIContent(s_RemoveIcon.Texture, "Clear note");
            s_EndEditContent = new GUIContent(s_EndEditIcon.Texture, "End editing note");
            s_AddContent = new GUIContent(s_AddIcon.Texture, "Add note");
            s_CopyContent = new GUIContent(s_CopyIcon.Texture, "Copy note text");
            s_PingContent = new GUIContent(s_PingIcon.Texture, "Ping note");
        }

        public static void ResetEdit()
        {
            s_EditingText = null;
        }

        private static bool CanDraw()
        {
            if (s_RichLargeLabel == null)
            {
                Initialize();
            }

            if (UNotesStorage.Instance == null)
            {
                GUILayout.Label("No NotesStorage, something went wrong...");
                return false;
            }

            return true;
        }

        public static int GetObjectID(Object target)
        {
            return PrefabUtility.IsPartOfAnyPrefab(target)
                ? PrefabUtility
                    .GetCorrespondingObjectFromSourceAtPath(target, PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(target))
                    .GetInstanceID()
                : target.GetInstanceID();
        }

        /// <summary>
        /// /// Drawing "Add Note" UI for object note
        /// </summary>
        /// <param name="objectID">Instance ID of an object</param>
        /// <param name="editing">Flag to check if we are editing a note</param>
        public static void DrawAddNoteGUI(int objectID, ref bool editing)
        {
            if (!CanDraw())
            {
                return;
            }

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(s_AddContent, EditorStyles.iconButton))
            {
                UNotesStorage.Instance.AddNote(new ObjectUNote(objectID, string.Empty));
                editing = true;
            }
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        /// Drawing "Add Note" UI for project note
        /// </summary>
        /// <param name="editing">Flag to check if we are editing a note</param>
        public static void DrawAddNoteGUI(ref bool editing)
        {
            if (!CanDraw())
            {
                return;
            }

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(s_AddContent, EditorStyles.iconButton))
            {
                UNotesStorage.Instance.AddNote(new UNote(string.Empty));
                editing = true;
            }
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        /// Drawing whole GUI of Inspector's header with a note
        /// </summary>
        /// <param name="objectID">Instance ID of an object</param>
        /// <param name="editing">Flag to check if we are editing a note</param>
        public static void DrawObjectNoteInspectorGUI(Object obj, ref bool editing)
        {
            if (!CanDraw())
            {
                return;
            }

            var objectID = GetObjectID(obj);

            if (UNotesStorage.Instance.TryGetObjectNote(obj, out var objectNote))
            {
                EditorGUILayout.BeginHorizontal();

                if (editing)
                {
                    DrawEditingGUI(objectNote, ref editing);
                }
                else
                {
                    DrawNoteTextLabel(objectNote);

                    EditorGUILayout.Space();

                    DrawInspectorNoteButtons(objectNote);
                    DrawDefaultNoteButtons(objectNote, ref editing);
                }
                EditorGUILayout.EndHorizontal();
            }
            else
            {
                DrawAddNoteGUI(objectID, ref editing);
            }
        }

        /// <summary>
        /// Single line GUI of a note
        /// </summary>
        /// <param name="note">Note which GUI is drawing</param>
        /// <param name="lineWidth">Width of a GUI line</param>
        /// <param name="editing">Flag to check if we are editing a note</param>
        public static void DrawNoteInlineGUI(UNote note, float lineWidth, ref bool editing)
        {
            if (!CanDraw())
            {
                return;
            }

            EditorGUILayout.BeginHorizontal();

            if (editing)
            {
                DrawEditingGUI(note, ref editing);
            }
            else
            {
                DrawSimpleGUI(note, ref editing);
            }
            EditorGUILayout.EndHorizontal();
        }

        private static void DrawNoteTextLabel(UNote note)
        {
            var text = note.NoteText == "" ?
                k_NotePlaceholderText :
                note.NoteText;
            DrawTextLabel(text);
        }

        private static void DrawTextLabel(string text)
        {
            var content = new GUIContent(text);
            var size = s_RichLargeLabel.CalcSize(content);
            EditorGUILayout.LabelField(content, s_RichLargeLabel, GUILayout.Width(size.x));
        }

        private static void DrawInspectorNoteButtons(ObjectUNote note)
        {
            if (!UNotesWindow.Exists 
                && GUILayout.Button(s_PingContent, EditorStyles.iconButton))
            {
                UNotesWindow.CurrentNotesType = UNotesWindow.NotesType.Objects;
                UNotesWindow.ShowWindow();
            }
        }

        private static void DrawDefaultNoteButtons(UNote note, ref bool editing)
        {
            if (string.IsNullOrWhiteSpace(note.NoteText))
            {
                GUI.enabled = false;
            }

            if (GUILayout.Button(s_CopyContent, EditorStyles.iconButton))
            {
                EditorGUIUtility.systemCopyBuffer = note.NoteText;
            }
            GUI.enabled = true;

            if (GUILayout.Button(s_EditContent, EditorStyles.iconButton))
            {
                editing = true;
            }
            if (GUILayout.Button(s_RemoveEditContent, EditorStyles.iconButton))
            {
                UNotesStorage.Instance.RemoveNote(note);
            }
        }

        private static void DrawInlineNoteButtons(UNote note)
        {
            if (note is ObjectUNote objectNote
                && GUILayout.Button(s_PingContent, EditorStyles.iconButton))
            {
                var obj = objectNote.GetObject();
                EditorGUIUtility.PingObject(obj);
                Selection.activeObject = obj;
            }
        }
        
        private static void DrawEditingGUI(UNote note, ref bool editing)
        {
            if (s_EditingText == null)
            {
                s_EditingText = note.NoteText;
            }
            s_EditingText = EditorGUILayout.TextField(s_EditingText);

            if (GUILayout.Button(s_EndEditContent, EditorStyles.iconButton))
            {
                if (s_EditingText != note.NoteText)
                {
                    note.NoteText = s_EditingText;
                    UNotesStorage.Instance.Save();
                }
                s_EditingText = null;
                editing = false;
            }
        }

        private static void DrawSimpleGUI(UNote note, ref bool editing)
        {
            if (note is ObjectUNote objectNote)
            {
                var obj = objectNote.GetObject();
                if (obj == null)
                {
                    EditorGUILayout.EndHorizontal();
                }
                else
                {
                    var noteAssetIconContent = new GUIContent(AssetPreview.GetMiniThumbnail(obj));
                    EditorGUILayout.LabelField(noteAssetIconContent, s_RichLargeLabel, GUILayout.Width(20), GUILayout.Height(20));
                    DrawTextLabel(obj.name);
                    DrawTextLabel("<b>►</b>");
                }
            }

            DrawNoteTextLabel(note);

            GUILayout.FlexibleSpace();

            DrawInlineNoteButtons(note);
            DrawDefaultNoteButtons(note, ref editing);
        }

        /// <summary>
        /// Utility class to get Built-in icon's textures
        /// </summary>
        [System.Serializable]
        public class BuiltInEditorIcon
        {
            public string name;
            public Texture Texture
            {
                get
                {
                    if (texture == null)
                    {
                        texture = EditorGUIUtility.FindTexture(name);
                    }

                    return texture;
                }
            }

            private Texture texture;

            public BuiltInEditorIcon(string name)
            {
                this.name = name;
            }
        }
    }
}

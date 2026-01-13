using UnityEditor;
using UnityEngine;

namespace UNotes
{
    /// <summary>
    /// Basic note
    /// </summary>
    [System.Serializable]
    public class UNote
    {
        [SerializeField] private string m_NoteText;

        /// <summary>
        /// Current note text
        /// </summary>
        public string NoteText { get => m_NoteText; internal set => m_NoteText = value; }

        public UNote(string noteText)
        {
            NoteText = noteText;
        }
    }

    /// <summary>
    /// Note that connected to a specific object
    /// </summary>
    [System.Serializable]
    public class ObjectUNote : UNote, ISerializationCallbackReceiver
    {
        [SerializeField] private string m_GlobalObjectIDSerialization;
        private GlobalObjectId m_GlobalObjectID;
        private int? m_EntityID;

        /// <summary>
        /// Object ID that note is connected to
        /// </summary>
        public int EntityID
        {
            get
            {
                if (m_EntityID == null || EditorUtility.EntityIdToObject((int)m_EntityID) == null)
                {
                    m_EntityID = GlobalObjectId.GlobalObjectIdentifierToEntityIdSlow(m_GlobalObjectID);
                }

                return m_EntityID ?? default;
            }
        }

        public ObjectUNote(int objectID, string noteText) :
            this(EditorUtility.EntityIdToObject(objectID), noteText)
        { }

        public ObjectUNote(Object attachedObject, string noteText) : base(noteText)
        {
            if (attachedObject != null)
            {
                m_GlobalObjectID = GlobalObjectId.GetGlobalObjectIdSlow(attachedObject);
            }
        }

        public void OnBeforeSerialize()
        {
            m_GlobalObjectIDSerialization = m_GlobalObjectID.ToString();
        }

        public void OnAfterDeserialize()
        {
            GlobalObjectId.TryParse(m_GlobalObjectIDSerialization, out m_GlobalObjectID);
            m_EntityID = null;
        }

        /// <summary>
        /// Get object that note is connected to
        /// </summary>
        /// <returns></returns>
        public Object GetObject()
        {
            return EditorUtility.EntityIdToObject(EntityID);
        }
    }
}

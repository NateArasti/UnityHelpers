using UnityEngine;

namespace HelperFunctions
{
    public static class UIExtensions
    {
        /// <summary>
        /// Set pivot without moving the element
        /// </summary>
        public static void SetPivot(this RectTransform rect, Vector2 pivot)
        {
            var offset = pivot - rect.pivot;
            offset.Scale(rect.rect.size);
            var worldPos = rect.position + rect.TransformVector(offset);
            rect.pivot = pivot;
            rect.position = worldPos;
        }

        /// <summary>
        /// Stretch to fill parent
        /// </summary>
        public static void StretchToFillParent(this RectTransform rect)
        {
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.sizeDelta = Vector2.zero;
            rect.anchoredPosition = Vector2.zero;
        }
    }
}

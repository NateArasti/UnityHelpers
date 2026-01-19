using UnityEngine;

namespace HelperFunctions
{
    public static class GameObjectExtensions
    {
        /// <summary>
        /// Set layer for transform and all children
        /// </summary>
        public static void SetLayerDeep(this GameObject obj, int layer)
        {
            obj.layer = layer;
            foreach (Transform child in obj.transform)
            {
                child.gameObject.SetLayerDeep(layer);
            }
        }

        /// <summary>
        /// Set active state for all children
        /// </summary>
        public static void SetChildrenActive(this GameObject obj, bool active)
        {
            foreach (Transform child in obj.transform)
            {
                child.gameObject.SetActive(active);
            }
        }

        /// <summary>
        /// Get path from root to this transform
        /// </summary>
        public static string GetPath(this GameObject obj)
        {
            var path = obj.name;
            var parent = obj.transform.parent;
            while (parent != null)
            {
                path = parent.gameObject.name + "/" + path;
                parent = parent.parent;
            }
            return path;
        }

        /// <summary>
        /// Toggles self active state of object
        /// </summary>
        public static void ToggleActive(this GameObject obj)
        {
            obj.SetActive(!obj.activeSelf);
        }

        /// <summary>
        /// Gets provided component or adds if doesn't exist
        /// </summary>
        public static T GetOrAddComponent<T>(this GameObject obj) where T : Component
        {
            if (!obj.TryGetComponent<T>(out var component))
            {
                component = obj.AddComponent<T>();
            }
            return component;
        }

        /// <summary>
        /// Checks if object has component
        /// </summary>
        public static bool Has<T>(this GameObject obj) where T : Component
        {
            return obj.TryGetComponent<T>(out _);
        }
    }
}

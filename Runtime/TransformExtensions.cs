using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace HelperFunctions
{
    public static class TransformExtensions
    {
        /// <summary>
        /// Detach all children
        /// </summary>
        public static void DetachChildren(this Transform transform)
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
            {

                transform.GetChild(i).SetParent(null);
            }
        }

        /// <summary>
        /// Copy local transform values from another transform
        /// </summary>
        public static void CopyLocal(this Transform transform, Transform source)
        {
            transform.localPosition = source.localPosition;
            transform.localRotation = source.localRotation;
            transform.localScale = source.localScale;
        }

        /// <summary>
        /// Copy world transform values from another transform
        /// </summary>
        public static void CopyWorld(this Transform transform, Transform source)
        {
            transform.position = source.position;
            transform.rotation = source.rotation;
        }

        /// <summary>
        /// Resets local position, rotation and scale to default values
        /// </summary>
        public static void ResetLocally(
            this Transform transform,
            bool resetPosition = true,
            bool resetRotation = true,
            bool resetScale = true
            )
        {
            if (resetPosition)
            {
                transform.localPosition = Vector3.zero;
            }
            if (resetRotation)
            {
                transform.localRotation = Quaternion.identity;
            }
            if (resetScale)
            {
                transform.localScale = Vector3.one;
            }
        }

        /// <summary>
        /// Resets world position and rotation to default values
        /// </summary>
        public static void ResetWorld(
            this Transform transform,
            bool resetPosition = true,
            bool resetRotation = true
        )
        {
            if (resetPosition)
            {
                transform.position = Vector3.zero;
            }
            if (resetRotation)
            {
                transform.rotation = Quaternion.identity;
            }
        }

        /// <summary>
        /// Destroy all kids of object with certain delay
        /// </summary>
        public static void DestroyChildren(this Transform transform, float delayBetween = 0)
        {
            var count = 1;
            foreach (Transform childTransform in transform)
            {
                Object.Destroy(childTransform.gameObject, delayBetween * count);
                count++;
            }
        }

        /// <summary>
        /// For each object from the given collection, spawns an instance of given prefab.
        /// Also stores everything in dictionary and invoke specified action on spawn.
        /// TIP: Useful for generic scrolls in UI
        /// </summary>
        public static IReadOnlyDictionary<T2, T1> SpawnPrefabForEachObjectInCollection<T1, T2>(
            this Transform pivot,
            Func<T1> itemFactory,
            IEnumerable<T2> collection,
            UnityAction<T1, T2> spawnEvent = null,
            bool destroyChildren = false) where T1 : MonoBehaviour
        {
            if (destroyChildren)
            {
                pivot.DestroyChildren();
            }

            var spawnedObjects = new Dictionary<T2, T1>();

            foreach (var item in collection)
            {
                if (spawnedObjects.ContainsKey(item))
                {
                    continue;
                }

                var prefabInstance = itemFactory();
                spawnedObjects.Add(item, prefabInstance);
                spawnEvent?.Invoke(prefabInstance, item);
            }

            return spawnedObjects;
        }
        
        /// <summary>
        /// Get all children transforms
        /// </summary>
        public static List<Transform> GetChildren(this Transform transform)
        {
            var children = new List<Transform>();
            foreach (Transform child in transform)
            {
                children.Add(child);
            }

            return children;
        }

        /// <summary>
        /// Get all children recursively
        /// </summary>
        public static List<Transform> GetChildrenDeep(this Transform transform)
        {
            var children = new List<Transform>();
            foreach (Transform child in transform)
            {
                children.Add(child);
                children.AddRange(child.GetChildrenDeep());
            }
            return children;
        }

        /// <summary>
        /// Find child by name recursively
        /// </summary>
        public static Transform FindDeep(this Transform transform, string name)
        {
            if (transform.name == name)
            {
                return transform;
            }

            foreach (Transform child in transform)
            {
                var result = child.FindDeep(name);
                if (result != null)
                {
                    return result;
                }
            }
            return null;
        }

        /// <summary>
        /// Execute action for each child
        /// </summary>
        public static void ForEachChild(this Transform transform, UnityAction<Transform> action)
        {
            foreach (Transform child in transform)
            {
                action(child);
            }
        }

        /// <summary>
        /// Execute action for each child recursively
        /// </summary>
        public static void ForEachChildDeep(this Transform transform, UnityAction<Transform> action)
        {
            foreach (Transform child in transform)
            {
                action(child);
                child.ForEachChildDeep(action);
            }
        }

        /// <summary>
        /// Get distance to another transform
        /// </summary>
        public static float DistanceTo(this Transform transform, Transform other)
        {
            return Vector3.Distance(transform.position, other.position);
        }

        /// <summary>
        /// Check if this transform is child of parent (recursively)
        /// </summary>
        public static bool IsChildOf(this Transform transform, Transform parent)
        {
            var current = transform.parent;
            while (current != null)
            {
                if (current == parent)
                {
                    return true;
                }

                current = current.parent;
            }
            return false;
        }
    }
}

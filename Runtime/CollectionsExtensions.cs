using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace HelperFunctions
{
    public static class CollectionsExtensions
    {
        #region Common

        /// <summary>
        /// Check if collection is null or empty
        /// </summary>
        public static bool IsNullOrEmpty<T>(this IReadOnlyCollection<T> collection)
        {
            return collection == null || collection.Count == 0;
        }

        ///<summary>
        /// Call Action for each object in collection
        ///</summary>
        public static void ForEachAction<T>(this IEnumerable<T> collection, UnityAction<T> action)
        {
            foreach (var obj in collection)
            {
                action.Invoke(obj);
            }
        }

        ///<summary>
        /// Trying to find object in collection
        ///</summary>
        public static bool TryGetObject<T1, T2>(this IEnumerable<T1> collection,
            T2 compareValue, Func<T1, T2, bool> compareFunction, out T1 result)
        {
            foreach (var obj in collection)
            {
                if (!compareFunction(obj, compareValue))
                {
                    continue;
                }


                result = obj;
                return true;
            }

            result = default;
            return false;
        }

        /// <summary>
        /// Swap two elements in list
        /// </summary>
        public static void Swap<T>(this IList<T> list, int indexA, int indexB)
        {
            (list[indexA], list[indexB]) = (list[indexB], list[indexA]);
        }

        /// <summary>
        /// Remove all items matching predicate
        /// </summary>
        public static int RemoveAll<T>(this IList<T> list, Predicate<T> match)
        {
            var count = 0;
            for (var i = list.Count - 1; i >= 0; --i)
            {
                if (match(list[i]))
                {
                    list.RemoveAt(i);
                    count++;
                }
            }
            return count;
        }

        /// <summary>
        /// Get or add value to dictionary
        /// </summary>
        public static TValue GetOrAdd<TKey, TValue>(
            this IDictionary<TKey, TValue> dict,
            TKey key, Func<TValue> valueFactory)
        {
            if (!dict.TryGetValue(key, out var value))
            {
                value = valueFactory();
                dict[key] = value;
            }
            return value;
        }

        /// <summary>
        /// Try remove and return removed value
        /// </summary>
        public static bool TryRemove<TKey, TValue>(
            this IDictionary<TKey, TValue> dict,
            TKey key, out TValue value)
        {
            if (dict.TryGetValue(key, out value))
            {
                dict.Remove(key);
                return true;
            }
            return false;
        }

        #endregion

        #region Random

        ///<summary>
        /// Gets random object from list
        ///</summary>
        public static T GetRandomElement<T>(this IReadOnlyList<T> list, out int index)
        {
            if (list.IsNullOrEmpty())
            {
                throw new UnityException("Can't get random object from empty list");
            }

            index = Random.Range(0, list.Count);

            return list[index];
        }

        ///<summary>
        /// Gets random object from collection
        /// O(rand_index)
        ///</summary>
        public static T GetRandomElement<T>(this IReadOnlyCollection<T> collection, out int index)
        {
            if (collection.IsNullOrEmpty())
            {
                throw new UnityException("Can't get random object from an empty collection");
            }

            index = Random.Range(0, collection.Count);
            var count = 0;
            foreach (var obj in collection)
            {
                if (count == index)
                {
                    return obj;
                }

                count += 1;
            }

            return default;
        }

        /// <summary>
        /// Get random elements from list without duplicates
        /// </summary>
        public static IEnumerable<T> GetRandomElements<T>(this IReadOnlyList<T> list, int count)
        {
            if (list.IsNullOrEmpty())
            {
                throw new UnityException("Can't get random elements from an empty collection");
            }

            if (count > list.Count)
                throw new UnityException("Cannot get more elements than list contains");
            if (count <= 0)
                throw new UnityException("Count must be positive");

            var indices = new HashSet<int>();
            while (indices.Count < count)
            {
                var index = Random.Range(0, list.Count);
                if (indices.Add(index))
                    yield return list[index];
            }
        }

        #endregion

        #region Shuffle

        /// <summary>
        /// Shuffle list in place using Fisher-Yates algorithm
        /// </summary>
        public static void Shuffle<T>(this IList<T> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }

        /// <summary>
        /// Get shuffled copy using Fisher-Yates
        /// </summary>
        public static List<T> GetShuffled<T>(this IReadOnlyList<T> list)
        {
            var newList = new List<T>(list);
            newList.Shuffle();
            return newList;
        }

        #endregion
    }
}

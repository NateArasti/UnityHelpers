using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;

namespace HelperFunctions.Tests.Editor
{
    public class CollectionsExtensionsTests
    {
        [Test]
        public void IsNullOrEmpty_Null_ReturnsTrue()
        {
            List<int> list = null;
            Assert.IsTrue(list.IsNullOrEmpty());
        }

        [Test]
        public void IsNullOrEmpty_Empty_ReturnsTrue()
        {
            var list = new List<int>();
            Assert.IsTrue(list.IsNullOrEmpty());
        }

        [Test]
        public void IsNullOrEmpty_NotEmpty_ReturnsFalse()
        {
            var list = new List<int> { 1 };
            Assert.IsFalse(list.IsNullOrEmpty());
        }

        [Test]
        public void ForEachAction_CallsActionForEachElement()
        {
            var list = new List<int> { 1, 2, 3 };
            var sum = 0;

            list.ForEachAction(i => sum += i);

            Assert.AreEqual(6, sum);
        }

        [Test]
        public void TryGetObject_FindsMatchingElement()
        {
            var list = new List<int> { 1, 2, 3 };

            var found = list.TryGetObject(2, (x, v) => x == v, out var result);

            Assert.IsTrue(found);
            Assert.AreEqual(2, result);
        }

        [Test]
        public void TryGetObject_NoMatch_ReturnsFalse()
        {
            var list = new List<int> { 1, 2, 3 };

            var found = list.TryGetObject(5, (x, v) => x == v, out var result);

            Assert.IsFalse(found);
            Assert.AreEqual(default(int), result);
        }

        [Test]
        public void Swap_SwapsElements()
        {
            var list = new List<int> { 1, 2 };

            list.Swap(0, 1);

            Assert.AreEqual(2, list[0]);
            Assert.AreEqual(1, list[1]);
        }

        [Test]
        public void RemoveAll_RemovesMatchingItems()
        {
            var list = new List<int> { 1, 2, 3, 4 };

            var removed = list.RemoveAll(i => i % 2 == 0);

            Assert.AreEqual(2, removed);
            CollectionAssert.AreEquivalent(new[] { 1, 3 }, list);
        }

        [Test]
        public void GetOrAdd_AddsWhenMissing()
        {
            var dict = new Dictionary<string, int>();

            var value = dict.GetOrAdd("a", () => 5);

            Assert.AreEqual(5, value);
            Assert.AreEqual(5, dict["a"]);
        }

        [Test]
        public void GetOrAdd_ReturnsExistingValue()
        {
            var dict = new Dictionary<string, int> { { "a", 3 } };

            var value = dict.GetOrAdd("a", () => 5);

            Assert.AreEqual(3, value);
        }

        [Test]
        public void TryRemove_RemovesExistingKey()
        {
            var dict = new Dictionary<string, int> { { "a", 1 } };

            var removed = dict.TryRemove("a", out var value);

            Assert.IsTrue(removed);
            Assert.AreEqual(1, value);
            Assert.IsFalse(dict.ContainsKey("a"));
        }

        [Test]
        public void TryRemove_MissingKey_ReturnsFalse()
        {
            var dict = new Dictionary<string, int>();

            var removed = dict.TryRemove("a", out var value);

            Assert.IsFalse(removed);
            Assert.AreEqual(default(int), value);
        }

        [Test]
        public void GetRandomElement_List_ReturnsElementAndValidIndex()
        {
            Random.InitState(123);
            var list = new List<int> { 10, 20, 30 };

            var element = list.GetRandomElement(out var index);

            Assert.IsTrue(index >= 0 && index < list.Count);
            Assert.AreEqual(list[index], element);
        }

        [Test]
        public void GetRandomElement_List_Empty_Throws()
        {
            var list = new List<int>();

            Assert.Throws<UnityException>(() =>
                list.GetRandomElement(out _));
        }

        [Test]
        public void GetRandomElement_Collection_ReturnsElement()
        {
            Random.InitState(456);
            var collection = new HashSet<int> { 1, 2, 3 };

            var element = collection.GetRandomElement(out var index);

            Assert.IsTrue(index >= 0 && index < collection.Count);
            Assert.Contains(element, collection.ToList());
        }

        [Test]
        public void GetRandomElements_ReturnsCorrectCount_NoDuplicates()
        {
            Random.InitState(789);
            var list = new List<int> { 1, 2, 3, 4, 5 };

            var result = list.GetRandomElements(3).ToList();

            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(3, result.Distinct().Count());
        }

        [Test]
        public void GetRandomElements_InvalidCount_Throws()
        {
            var list = new List<int> { 1, 2 };

            Assert.Throws<UnityException>(() => list.GetRandomElements(0).ToList());
            Assert.Throws<UnityException>(() => list.GetRandomElements(3).ToList());
        }

        [Test]
        public void Shuffle_PreservesElements()
        {
            Random.InitState(321);
            var list = new List<int> { 1, 2, 3, 4 };

            list.Shuffle();

            CollectionAssert.AreEquivalent(new[] { 1, 2, 3, 4 }, list);
        }

        [Test]
        public void GetShuffled_ReturnsNewList()
        {
            Random.InitState(654);
            var list = new List<int> { 1, 2, 3 };

            var shuffled = list.GetShuffled();

            Assert.AreNotSame(list, shuffled);
            CollectionAssert.AreEquivalent(list, shuffled);
        }
    }
}

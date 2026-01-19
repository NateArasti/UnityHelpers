using NUnit.Framework;
using System.Collections;
using UnityEngine.TestTools;

namespace HelperFunctions.Tests
{
    public class CoroutineExtensionsTests
    {
        private const float Epsilon = 0.0001f;

        [UnityTest]
        public IEnumerator InvokeAfter_StaticSeparateBehaviour_InvokesAction()
        {
            var invoked = false;

            yield return CoroutineExtensions.InvokeAfter(() => true, () => invoked = true);

            Assert.IsTrue(invoked);
        }

        [UnityTest]
        public IEnumerator InvokeWhile_StaticSeparateBehaviour_InvokesMultipleTimes()
        {
            const int maxCount = 3;
            var count = 0;

            yield return CoroutineExtensions.InvokeWhile(() => count < maxCount, () => count++);

            Assert.AreEqual(maxCount, count);
        }

        [UnityTest]
        public IEnumerator InvokeFramesDelayed_StaticSeparateBehaviour_InvokesAfterFrames()
        {
            const int delayFrames = 2;
            var invoked = false;

            yield return CoroutineExtensions.InvokeFramesDelayed(() => invoked = true, delayFrames);

            Assert.IsTrue(invoked);
        }

        [UnityTest]
        public IEnumerator InvokeNextFrame_StaticSeparateBehaviour_InvokesOnNextFrame()
        {
            var invoked = false;

            yield return CoroutineExtensions.InvokeNextFrame(() => invoked = true);

            Assert.IsTrue(invoked);
        }

        [UnityTest]
        public IEnumerator InvokeEndOfFrame_StaticSeparateBehaviour_InvokesAtEndOfFrame()
        {
            var invoked = false;

            yield return CoroutineExtensions.InvokeEndOfFrame(() => invoked = true);

            Assert.IsTrue(invoked);
        }

        [UnityTest]
        public IEnumerator InvokeFixedUpdate_StaticSeparateBehaviour_InvokesOnNextFixedUpdate()
        {
            var invoked = false;

            yield return CoroutineExtensions.InvokeFixedUpdate(() => invoked = true);

            Assert.IsTrue(invoked);
        }

        [UnityTest]
        public IEnumerator InvokeSecondsDelayed_StaticSeparateBehaviour_InvokesAfterDelay()
        {
            const float delay = 0.1f;
            var invoked = false;

            yield return CoroutineExtensions.InvokeSecondsDelayed(() => invoked = true, delay);

            Assert.IsTrue(invoked);
        }

        [UnityTest]
        public IEnumerator InvokeRealtimeSecondsDelayed_StaticSeparateBehaviour_InvokesAfterDelay()
        {
            const float delay = 0.1f;
            var invoked = false;

            yield return CoroutineExtensions.InvokeRealtimeSecondsDelayed(() => invoked = true, delay);

            Assert.IsTrue(invoked);
        }

        [UnityTest]
        public IEnumerator InvokeRepeating_StaticSeparateBehaviour_InvokesMultipleTimes()
        {
            const int repeat = 3;
            const float interval = 0.05f;

            var count = 0;
            yield return CoroutineExtensions.InvokeRepeating(() => count++, interval, repeat);

            Assert.AreEqual(repeat, count);
        }

        [UnityTest]
        public IEnumerator ChainActions_StaticSeparateBehaviour_InvokesAllInOrder()
        {
            var log = new System.Text.StringBuilder();

            yield return CoroutineExtensions.ChainActions(
                ( () => log.Append("A"), 1f ),
                ( () => log.Append("B"), 1f ),
                ( () => log.Append("C"), 1f )
            );

            Assert.AreEqual("ABC", log.ToString());
        }

        [UnityTest]
        public IEnumerator TryStopCoroutine_StaticSeparateBehaviour_SetsReferenceToNull()
        {
            var invoked = false;
            var coroutine = CoroutineExtensions.InvokeNextFrame(() => invoked = true);

            CoroutineExtensions.TryStopCoroutine(ref coroutine);

            Assert.IsNull(coroutine);
            Assert.IsFalse(invoked);

            yield break;
        }

        [UnityTest]
        public IEnumerator TryStopCoroutine_StaticSeparateBehaviour_StopWithoutReference_DoesNotThrow()
        {
            var invoked = false;
            var coroutine = CoroutineExtensions.InvokeNextFrame(() => invoked = true);

            CoroutineExtensions.TryStopCoroutine(coroutine);

            Assert.IsFalse(invoked);
            Assert.IsNotNull(CoroutineExtensions.SeparateCoroutineBehaviour); // SeparateBehaviour still exists
            
            yield break;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HelperFunctions
{
    public static class CoroutineExtensions
    {
        #region Caching

        private static readonly Dictionary<float, WaitForSeconds> s_WaitSecondsDictionary = new();
        private static readonly Dictionary<float, WaitForSecondsRealtime> s_WaitSecondsRealtimeDictionary = new();

        /// <summary>
        /// Non-allocated WaitForFixedUpdate
        /// </summary>
        public static WaitForFixedUpdate WaitForFixedUpdate { get; } = new WaitForFixedUpdate();

        /// <summary>
        /// Non-allocated WaitForEndOfFrame (actually just null)
        /// </summary>
        public static WaitForEndOfFrame WaitForEndOfFrame { get; } = null; // yield return null is safe in batch mode

        ///<summary>
        /// Non-allocated WaitForSeconds
        ///</summary>
        public static WaitForSeconds WaitSeconds(float waitTime)
        {
            if (s_WaitSecondsDictionary.TryGetValue(waitTime, out var waitForSeconds))
            {
                return waitForSeconds;
            }

            s_WaitSecondsDictionary[waitTime] = new WaitForSeconds(waitTime);
            return s_WaitSecondsDictionary[waitTime];
        }

        ///<summary>
        /// Non-allocated WaitForSecondsRealtime
        ///</summary>
        public static WaitForSecondsRealtime WaitSecondsRealtime(float waitTime)
        {
            if (s_WaitSecondsRealtimeDictionary.TryGetValue(waitTime, out var waitForSeconds))
            {

                return waitForSeconds;
            }

            s_WaitSecondsRealtimeDictionary[waitTime] = new WaitForSecondsRealtime(waitTime);
            return s_WaitSecondsRealtimeDictionary[waitTime];
        }

        #endregion

        #region Coroutine Invoking

        public class CoroutineBehaviour : MonoBehaviour
        {
        }

        private static CoroutineBehaviour s_SeparateCoroutineBehaviour;
        /// <summary>
        /// This is behaviour that is running all coroutines that were invoked separately
        /// </summary>
        public static CoroutineBehaviour SeparateCoroutineBehaviour
        {
            get
            {
                if (s_SeparateCoroutineBehaviour == null)
                {
                    s_SeparateCoroutineBehaviour = new GameObject(
                        "---- Separate Behaviour For Coroutine ----",
                        typeof(CoroutineBehaviour)
                    ).GetComponent<CoroutineBehaviour>();
                    UnityEngine.Object.DontDestroyOnLoad(s_SeparateCoroutineBehaviour);
                    s_SeparateCoroutineBehaviour.gameObject.hideFlags = HideFlags.HideAndDontSave;
                }
                return s_SeparateCoroutineBehaviour;
            }
        }

        private static Coroutine StartCoroutine(
            MonoBehaviour behaviour,
            IEnumerator enumerator)
        {
            return behaviour.StartCoroutine(enumerator);
        }

        #endregion

        #region Predicates

        private static IEnumerator AfterDelay(UnityAction action, Func<bool> predicate)
        {
            yield return new WaitUntil(predicate);
            action.Invoke();
        }

        private static IEnumerator WhileInvoke(Func<bool> predicate, UnityAction action, float interval)
        {
            while (predicate())
            {
                action.Invoke();
                if (interval > 0f)
                {
                    yield return WaitSeconds(interval);
                }
                else
                {
                    yield return null;
                }
            }
        }

        /// <summary>
        /// Invoking action after predicate returns true
        /// </summary>
        /// <param name="behaviour"></param>
        /// <param name="predicate"></param>
        /// <param name="action"></param>
        public static Coroutine InvokeAfter(
            this MonoBehaviour behaviour,
            Func<bool> predicate,
            UnityAction action
            )
        {
            return StartCoroutine(
                behaviour,
                AfterDelay(action, predicate)
            );
        }

        /// <summary>
        /// Invoking action after predicate returns true on a separate behaviour
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="action"></param>
        public static Coroutine InvokeAfter(Func<bool> predicate, UnityAction action)
        {
            return SeparateCoroutineBehaviour.InvokeAfter(predicate, action);
        }

        /// <summary>
        /// Invoke action while predicate returns true
        /// </summary>
        public static Coroutine InvokeWhile(
            this MonoBehaviour behaviour,
            Func<bool> predicate,
            UnityAction action,
            float interval = 0f)
        {
            return StartCoroutine(
                behaviour,
                WhileInvoke(predicate, action, interval)
            );
        }

        /// <summary>
        /// Invoke action while predicate returns true
        /// </summary>
        public static Coroutine InvokeWhile(
            Func<bool> predicate,
            UnityAction action,
            float interval = 0f)
        {
            return SeparateCoroutineBehaviour.InvokeWhile(predicate, action, interval);
        }

        #endregion

        #region Frames

        private static IEnumerator FramesDelay(UnityAction action, int framesCountDelay)
        {
            for (var i = 0; i < framesCountDelay; ++i)
            {
                yield return null;
            }

            action.Invoke();
        }

        private static IEnumerator EndOfFrameDelay(UnityAction action)
        {
            yield return WaitForEndOfFrame;
            action.Invoke();
        }

        private static IEnumerator FixedUpdateDelay(UnityAction action)
        {
            yield return WaitForFixedUpdate;
            action.Invoke();
        }

        /// <summary>
        /// Invoking action after specified frames delay
        /// </summary>
        /// <param name="behaviour">Which behaviour will start delay coroutine</param>
        /// <param name="action">Action to invoke</param>
        /// <param name="framesCountDelay">Count of frames that will be </param>
        public static Coroutine InvokeFramesDelayed(
            this MonoBehaviour behaviour,
            UnityAction action,
            int framesCountDelay
            )
        {
            return StartCoroutine(
                behaviour,
                FramesDelay(action, framesCountDelay)
            );
        }

        /// <summary>
        /// Invoking action after specified frames delay on a separate behaviour
        /// </summary>
        /// <param name="action">Action to invoke</param>
        /// <param name="framesCountDelay">Count of frames that will be </param>
        public static Coroutine InvokeFramesDelayed(UnityAction action, int framesCountDelay)
        {
            return SeparateCoroutineBehaviour.InvokeFramesDelayed(action, framesCountDelay);
        }

        /// <summary>
        /// Invoke action on next frame
        /// </summary>
        public static Coroutine InvokeNextFrame(this MonoBehaviour behaviour, UnityAction action)
        {
            return behaviour.InvokeFramesDelayed(action, 1);
        }

        /// <summary>
        /// Invoke action on next frame
        /// </summary>
        public static Coroutine InvokeNextFrame(UnityAction action)
        {
            return InvokeFramesDelayed(action, 1);
        }

        /// <summary>
        /// Invoke action at end of current frame
        /// </summary>
        public static Coroutine InvokeEndOfFrame(this MonoBehaviour behaviour, UnityAction action)
        {
            return StartCoroutine(
                behaviour,
                EndOfFrameDelay(action)
            );
        }

        /// <summary>
        /// Invoke action at end of current frame
        /// </summary>
        public static Coroutine InvokeEndOfFrame(UnityAction action)
        {
            return SeparateCoroutineBehaviour.InvokeEndOfFrame(action);
        }

        /// <summary>
        /// Invoke action on next FixedUpdate
        /// </summary>
        public static Coroutine InvokeFixedUpdate(this MonoBehaviour behaviour, UnityAction action)
        {
            return StartCoroutine(
                behaviour,
                FixedUpdateDelay(action)
            );
        }

        /// <summary>
        /// Invoke action on next FixedUpdate
        /// </summary>
        public static Coroutine InvokeFixedUpdate(UnityAction action)
        {
            return SeparateCoroutineBehaviour.InvokeFixedUpdate(action);
        }

        #endregion

        #region Seconds

        private static IEnumerator SecondsDelay(UnityAction action, float delay)
        {
            yield return WaitSeconds(delay);
            action.Invoke();
        }

        private static IEnumerator RealtimeSecondsDelay(UnityAction action, float delay)
        {
            yield return WaitSecondsRealtime(delay);
            action.Invoke();
        }

        /// <summary>
        /// Invoking action after specified seconds delay
        /// </summary>
        /// <param name="behaviour">Which behaviour will start delay coroutine</param>
        /// <param name="action">Action to invoke</param>
        /// <param name="delay">Timed delay in seconds</param>
        public static Coroutine InvokeSecondsDelayed(
            this MonoBehaviour behaviour,
            UnityAction action,
            float delay
            )
        {
            return StartCoroutine(
                behaviour,
                SecondsDelay(action, delay)
            );
        }

        /// <summary>
        /// Invoking action after specified seconds delay on a separate behaviour
        /// </summary>
        /// <param name="action">Action to invoke</param>
        /// <param name="delay">Timed delay in seconds</param>
        public static Coroutine InvokeSecondsDelayed(UnityAction action, float delay)
        {
            return SeparateCoroutineBehaviour.InvokeSecondsDelayed(action, delay);
        }

        /// <summary>
        /// Invoking action after specified seconds delay in realtime
        /// </summary>
        /// <param name="behaviour">Which behaviour will start delay coroutine</param>
        /// <param name="action">Action to invoke</param>
        /// <param name="delay">Timed delay in realtime seconds</param>
        public static Coroutine InvokeRealtimeSecondsDelayed(
            this MonoBehaviour behaviour,
            UnityAction action,
            float delay
            )
        {
            return StartCoroutine(
                behaviour,
                RealtimeSecondsDelay(action, delay)
            );
        }

        /// <summary>
        /// Invoking action after specified seconds delay in realtime on a separate behaviour
        /// </summary>
        /// <param name="action">Action to invoke</param>
        /// <param name="delay">Timed delay in realtime seconds</param>
        public static Coroutine InvokeRealtimeSecondsDelayed(UnityAction action, float delay)
        {
            return SeparateCoroutineBehaviour.InvokeRealtimeSecondsDelayed(action, delay);
        }

        #endregion

        #region Cycles

        private static IEnumerator RepeatingInvoke(UnityAction action, float interval, int repeatCount, float initialDelay = 0)
        {
            if (initialDelay > 0f)
            {
                yield return WaitSeconds(initialDelay);
            }


            if (repeatCount < 0) // inf
            {
                while (true)
                {
                    action.Invoke();
                    yield return WaitSeconds(interval);
                }
            }
            else
            {
                for (int i = 0; i < repeatCount; i++)
                {
                    action.Invoke();
                    yield return WaitSeconds(interval);
                }
            }
        }

        private static IEnumerator ChainedActions(params (UnityAction action, float delay)[] actions)
        {
            foreach (var (action, delay) in actions)
            {
                if (delay > 0f)
                {
                    yield return WaitSeconds(delay);
                }

                action.Invoke();
            }
        }

        /// <summary>
        /// Invoke action repeatedly with interval
        /// </summary>
        public static Coroutine InvokeRepeating(
            this MonoBehaviour behaviour,
            UnityAction action,
            float interval,
            int repeatCount = -1,
            float initialDelay = 0f)
        {
            return StartCoroutine(
                behaviour,
                RepeatingInvoke(action, interval, repeatCount, initialDelay)
            );
        }

        /// <summary>
        /// Invoke action repeatedly with interval
        /// </summary>
        public static Coroutine InvokeRepeating(
            UnityAction action,
            float interval,
            int repeatCount = -1,
            float initialDelay = 0f)
        {
            return SeparateCoroutineBehaviour.InvokeRepeating(action, interval, repeatCount, initialDelay);
        }

        /// <summary>
        /// Chain multiple actions with delays
        /// </summary>
        public static Coroutine ChainActions(this MonoBehaviour behaviour, params (UnityAction action, float delay)[] actions)
        {
            return StartCoroutine(
                behaviour,
                ChainedActions(actions)
            );
        }

        /// <summary>
        /// Chain multiple actions with delays on separate behaviour
        /// </summary>
        public static Coroutine ChainActions(params (UnityAction action, float delay)[] actions)
        {
            return SeparateCoroutineBehaviour.ChainActions(actions);
        }

        #endregion

        #region Lifetime

        /// <summary>
        /// Safely stop a coroutine with null check
        /// </summary>
        public static void TryStopCoroutine(this MonoBehaviour behaviour, ref Coroutine coroutine)
        {
            if (coroutine != null && behaviour != null)
            {
                behaviour.StopCoroutine(coroutine);
                coroutine = null;
            }
        }

        /// <summary>
        /// Safely stop a coroutine with null check
        /// </summary>
        public static void TryStopCoroutine(this MonoBehaviour behaviour, Coroutine coroutine)
        {
            if (coroutine != null && behaviour != null)
            {
                behaviour.StopCoroutine(coroutine);
            }
        }

        /// <summary>
        /// Safely stop a coroutine with null check
        /// </summary>
        public static void TryStopCoroutine(ref Coroutine coroutine)
        {
            if (coroutine != null && SeparateCoroutineBehaviour != null)
            {
                SeparateCoroutineBehaviour.StopCoroutine(coroutine);
                coroutine = null;
            }
        }

        /// <summary>
        /// Safely stop a coroutine with null check
        /// </summary>
        public static void TryStopCoroutine(Coroutine coroutine)
        {
            if (coroutine != null && SeparateCoroutineBehaviour != null)
            {
                SeparateCoroutineBehaviour.StopCoroutine(coroutine);
            }
        }

        #endregion
    }
}

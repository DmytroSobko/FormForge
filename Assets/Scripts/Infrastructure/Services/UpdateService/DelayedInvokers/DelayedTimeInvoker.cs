using System;
using System.Collections;
using FormForge.UpdateService.Interfaces;
using UnityEngine;

namespace FormForge.UpdateService.DelayedInvokers
{
    internal struct DelayedTimeInvoker : IDelayedInvoker
    {
        private Action m_method;
        private WaitForSeconds m_waitForSeconds;

        /// <summary>
        /// A struct to contain and manage the method you wish to invoke after a time delay
        /// </summary>
        /// <param name="method">The method to be invoked</param>
        /// <param name="seconds">How long to delay the invocation in seconds</param>
        public DelayedTimeInvoker(Action method, float seconds)
        {
            m_method = method;
            m_waitForSeconds = new WaitForSeconds(seconds);
        }

        /// <summary>
        /// The Coroutine underlying the delayed invocation
        /// </summary>
        /// <returns>An IEnumerator of the coroutine for the delayed invocation</returns>
        public IEnumerator Delay()
        {
            yield return m_waitForSeconds;
            m_method?.Invoke();
        }
    }
}
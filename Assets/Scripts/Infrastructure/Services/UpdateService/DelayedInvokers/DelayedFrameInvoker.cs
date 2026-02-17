using System;
using System.Collections;
using FormForge.Coroutines.YieldInstructions;
using FormForge.UpdateService.Interfaces;

namespace FormForge.UpdateService.DelayedInvokers
{
    internal struct DelayedFrameInvoker : IDelayedInvoker
    {
        private Action m_method;
        private WaitForFrames m_waitForFrames;

        /// <summary>
        /// A struct to contain and manage the method you wish to invoke after a frame delay
        /// </summary>
        /// <param name="method">The method to be invoked</param>
        /// <param name="frames">How many frames to delay the invocation by</param>
        public DelayedFrameInvoker(Action method, int frames)
        {
            m_method = method;
            m_waitForFrames = new WaitForFrames(frames);
        }

        /// <summary>
        /// The Coroutine underlying the delayed invocation
        /// </summary>
        /// <returns>An IEnumerator of the coroutine for the delayed invocation</returns>
        public IEnumerator Delay()
        {
            yield return m_waitForFrames;
            m_method?.Invoke();
        }
    }
}
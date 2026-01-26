using System;
using System.Collections;
using UnityEngine;

namespace FormForge.UpdateService.Interfaces
{
    /// <summary>
    /// A service intended to give access to Unity Update based behaviours for plain C# objects 
    /// </summary>
    public interface IUpdateService
    {
        /// <summary>
        /// Registers an IUpdatable object to receive Update calls from Unity
        /// </summary>
        /// <param name="updatable">The object to update</param>
        /// <param name="frequency">How frequently you want to update</param>
        void Register(IUpdatable updatable, UpdateFrequency frequency);
        
        /// <summary>
        /// Unregisters an IUpdatable object so it no longer will receive Update calls from Unity
        /// </summary>
        /// <param name="updatable">The object to no longer update</param>
        void Unregister(IUpdatable updatable);
        
        /// <summary>
        /// Calls a method after a specified number of frames.
        /// </summary>
        /// <param name="method">The method to call</param>
        /// <param name="frameDelay">How many frames to delay the call by</param>
        /// <remarks>
        /// This method relies on a Coroutine under the hood
        /// </remarks>
        void DelayInvocationByFrames(Action method, int frameDelay);
        
        /// <summary>
        /// Calls a method after a specified amount of time in seconds.
        /// </summary>
        /// <param name="method">The method to call</param>
        /// <param name="timeDelay">How many seconds to delay the call by</param>
        /// <remarks>
        /// This method relies on a Coroutine under the hood
        /// </remarks>
        void DelayInvocationBySeconds(Action method, float timeDelay);
        
        /// <summary>
        /// Starts a coroutine. Typically used to run coroutines from non-Monobehaviours.
        /// </summary>
        /// <param name="coroutine">The coroutine's IEnumerator to run</param>
        /// <returns>An instance of your Coroutine</returns>
        Coroutine StartCoroutine(IEnumerator coroutine);
        
        /// <summary>
        /// Stops a coroutine that was started with the Updater's <see cref="StartCoroutine"/> method
        /// </summary>
        /// <param name="coroutine">The IEnumerator of the coroutine you wish to stop</param>
        void StopCoroutine(IEnumerator coroutine);
        
        /// <summary>
        /// Stops a coroutine that was started with the Updater's <see cref="StartCoroutine"/> method
        /// </summary>
        /// <param name="coroutine">The Coroutine you wish to stop</param>
        void StopCoroutine(Coroutine coroutine);
    }
}
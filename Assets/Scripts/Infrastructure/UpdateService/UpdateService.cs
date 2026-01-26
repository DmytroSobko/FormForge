using System;
using System.Collections;
using System.Collections.Generic;
using FormForge.Core.Services;
using FormForge.UpdateService.DelayedInvokers;
using FormForge.UpdateService.Interfaces;
using FormForge.Utils;
using UnityEngine;
using UnityEngine.Scripting;
using Object = UnityEngine.Object;

namespace FormForge.UpdateService
{
    /// <inheritdoc/>
    internal class UpdateService : IUpdateService
    {
        internal delegate void DontDestroyOnLoadDelegate(Object target);
        
        private Dictionary<UpdateFrequency, List<HashSet<WeakReferenceWrapper>>> m_updatables;
        private Dictionary<WeakReferenceWrapper, UpdateFrequency> m_updatableFrequencyMap;
        private Dictionary<UpdateFrequency, int> m_updateFrequencyIndex;

        private Updater m_updater;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void RegisterSelf()
        {
            ServiceLocator.RegisterService<IUpdateService, UpdateService>(ServiceLifespan.LazySingleton);
        }

        [Preserve]
        public UpdateService() : this(null)
        {
        }

        internal UpdateService(DontDestroyOnLoadDelegate dontDestroyOnLoadDelegate = null)
        {
            CreateUpdater(dontDestroyOnLoadDelegate);

            m_updatables = new Dictionary<UpdateFrequency, List<HashSet<WeakReferenceWrapper>>>();
            CreateUpdatablesList(UpdateFrequency.EveryFrame);
            CreateUpdatablesList(UpdateFrequency.EverySecondFrame);
            CreateUpdatablesList(UpdateFrequency.EveryFifthFrame);
            CreateUpdatablesList(UpdateFrequency.EveryFifteenthFrame);
            CreateUpdatablesList(UpdateFrequency.EveryThirtiethFrame);

            m_updateFrequencyIndex = new Dictionary<UpdateFrequency, int>
            {
                { UpdateFrequency.EveryFrame, 0 },
                { UpdateFrequency.EverySecondFrame, 0 },
                { UpdateFrequency.EveryFifthFrame, 0 },
                { UpdateFrequency.EveryFifteenthFrame, 0 },
                { UpdateFrequency.EveryThirtiethFrame, 0 }
            };

            m_updatableFrequencyMap = new Dictionary<WeakReferenceWrapper, UpdateFrequency>();
        }

        private void CreateUpdater(DontDestroyOnLoadDelegate dontDestroyOnLoadDelegate)
        {
            if (dontDestroyOnLoadDelegate == null)
            {
                dontDestroyOnLoadDelegate = Object.DontDestroyOnLoad;
            }
            
            m_updater = new GameObject("Updater").AddComponent<Updater>();
            dontDestroyOnLoadDelegate(m_updater.gameObject);

            m_updater.OnUpdate += OnUpdate;
        }

        private void OnUpdate()
        {
            UpdateUpdatables(UpdateFrequency.EveryFrame);
            UpdateUpdatables(UpdateFrequency.EverySecondFrame);
            UpdateUpdatables(UpdateFrequency.EveryFifthFrame);
            UpdateUpdatables(UpdateFrequency.EveryFifteenthFrame);
            UpdateUpdatables(UpdateFrequency.EveryThirtiethFrame);
        }

        private void UpdateUpdatables(UpdateFrequency frequency)
        {
            int updateIndex = m_updateFrequencyIndex[frequency];
            
            List<WeakReferenceWrapper> gcCollectedRefs = new List<WeakReferenceWrapper>();

            foreach (WeakReferenceWrapper weakUpdatable in m_updatables[frequency][updateIndex])
            {
                if (weakUpdatable.TryGetTarget(out IUpdatable updatable))
                {
                    if (updatable == null)
                    {
#if PRIME_DEBUG
                        throw new NullReferenceException("A registered Updatable object is null. Did an Updatable get destroyed without unregistering?");
#else
                        continue;
#endif
                    }
                    updatable.Update();
                }
                else
                {
                    gcCollectedRefs.Add(weakUpdatable);
                }
            }
            // Remove garbage collected weak references after iteration
            foreach (WeakReferenceWrapper weakUpdateable in gcCollectedRefs)
            {
                RemoveExistingRegistration(weakUpdateable);
            }
            m_updateFrequencyIndex[frequency] = (updateIndex + 1) % (int)frequency;
        }

        private void CreateUpdatablesList(UpdateFrequency frequency)
        {
            int frameCount = (int)frequency;
            List<HashSet<WeakReferenceWrapper>> newUpdatablesList = new List<HashSet<WeakReferenceWrapper>>(frameCount);

            for (int i = 0; i < frameCount; i++)
            {
                newUpdatablesList.Add(new HashSet<WeakReferenceWrapper>());
            }

            m_updatables.Add(frequency, newUpdatablesList);
        }

        public void Register(IUpdatable updatable, UpdateFrequency frequency)
        {
            var weakUpdateable = new WeakReferenceWrapper(updatable);
            RemoveExistingRegistration(weakUpdateable);
            var updatablesForFrequency = m_updatables[frequency];
            int leastPopulatedIndex = GetLeastPopulatedIndex(updatablesForFrequency);
            updatablesForFrequency[leastPopulatedIndex].Add(weakUpdateable);
            m_updatableFrequencyMap.Add(weakUpdateable, frequency);
        }

        private int GetLeastPopulatedIndex(List<HashSet<WeakReferenceWrapper>> updatablesForFrequency)
        {
            int leastPopulatedIndex = 0;
            for (int i = 0; i < updatablesForFrequency.Count; i++)
            {
                var list = updatablesForFrequency[i];
                if (list.Count < updatablesForFrequency[leastPopulatedIndex].Count)
                {
                    leastPopulatedIndex = i;

                    if (updatablesForFrequency[leastPopulatedIndex].Count == 0)
                    {
                        break;
                    }
                }
            }

            return leastPopulatedIndex;
        }

        public void Unregister(IUpdatable updatable)
        {
            WeakReferenceWrapper weakUpdateable = new WeakReferenceWrapper(updatable);
            RemoveExistingRegistration(weakUpdateable);
        }

        public void DelayInvocationByFrames(Action method, int frameDelay)
        {
            DelayedFrameInvoker delayedFrameInvoker = new DelayedFrameInvoker(method, frameDelay);
            m_updater.DelayedInvocation(delayedFrameInvoker);
        }

        public void DelayInvocationBySeconds(Action method, float timeDelay)
        {
            DelayedTimeInvoker delayedTimeInvoker = new DelayedTimeInvoker(method, timeDelay);
            m_updater.DelayedInvocation(delayedTimeInvoker);
        }

        public Coroutine StartCoroutine(IEnumerator coroutine)
        {
            return m_updater.StartCoroutine(coroutine);
        }

        public void StopCoroutine(IEnumerator coroutine)
        {
            m_updater.StopCoroutine(coroutine);
        }

        public void StopCoroutine(Coroutine coroutine)
        {
            m_updater.StopCoroutine(coroutine);
        }
        private void RemoveExistingRegistration(WeakReferenceWrapper weakUpdatable)
        {
            if (m_updatableFrequencyMap.TryGetValue(weakUpdatable, out var frequency))
            {
                var registeredUpdatables = m_updatables[frequency];
                foreach (HashSet<WeakReferenceWrapper> updatableObject in registeredUpdatables)
                {
                    if (updatableObject.Remove(weakUpdatable))
                    {
                        break;
                    }
                }

                m_updatableFrequencyMap.Remove(weakUpdatable);
            }
        }

        private class Updater : MonoBehaviour
        {
            public Action OnUpdate;

            private void Update()
            {
                OnUpdate?.Invoke();
            }

            public void DelayedInvocation(IDelayedInvoker invoker)
            {
                StartCoroutine(invoker.Delay());
            }
        }
    }
}
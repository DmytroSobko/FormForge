using System;
using System.Collections.Concurrent;
using System.Threading;
using Cysharp.Threading.Tasks;
using FormForge.Infrastructure.Services.Enums;
using UnityEngine;

namespace FormForge.Infrastructure.Services.CacheService
{
    public class CacheService : ICacheService
    {
        private class CacheEntry
        {
            public object Value;
            public DateTime Expiration;
            public readonly SemaphoreSlim Lock = new SemaphoreSlim(1, 1);
        }

        private readonly ConcurrentDictionary<string, CacheEntry> m_Cache = 
            new ConcurrentDictionary<string, CacheEntry>();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void RegisterSelf()
        {
            ServiceLocator.RegisterService<ICacheService, CacheService>(ServiceLifespan.LazySingleton);
        }
        
        public async UniTask<T> GetOrCreateAsync<T>(string key, Func<UniTask<T>> factory, TimeSpan lifetime)
        {
            var entry = m_Cache.GetOrAdd(key, _ => new CacheEntry());

            await entry.Lock.WaitAsync();
            try
            {
                if (entry.Value != null && DateTime.UtcNow < entry.Expiration)
                {
                    return (T)entry.Value;
                }

                var value = await factory();

                entry.Value = value;
                entry.Expiration = DateTime.UtcNow.Add(lifetime);

                return value;
            }
            finally
            {
                entry.Lock.Release();
            }
        }

        public bool TryGet<T>(string key, out T value)
        {
            value = default;

            if (!m_Cache.TryGetValue(key, out var entry))
            {
                return false;
            }

            if (entry.Value == null || DateTime.UtcNow >= entry.Expiration)
            {
                return false;
            }
            value = (T)entry.Value;
            return true;
        }

        public void Set<T>(string key, T value, TimeSpan lifetime)
        {
            var entry = m_Cache.GetOrAdd(key, _ => new CacheEntry());

            entry.Value = value;
            entry.Expiration = DateTime.UtcNow.Add(lifetime);
        }

        public void Update<T>(string key, Func<T, T> updateFunc)
        {
            if (!m_Cache.TryGetValue(key, out var entry))
            {
                return;
            }
            if (entry.Value is T typedValue)
            {
                entry.Value = updateFunc(typedValue);
            }
        }

        public void Invalidate(string key)
        {
            m_Cache.TryRemove(key, out _);
        }

        public void Clear()
        {
            m_Cache.Clear();
        }
    }
}
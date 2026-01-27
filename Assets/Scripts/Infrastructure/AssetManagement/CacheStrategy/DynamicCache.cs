using System.Collections.Concurrent;
using FormForge.AssetManagement.AssetPolicy;
using UnityEngine;

namespace FormForge.AssetManagement.CacheStrategy
{
    /// <summary>
    /// Implements a dynamic caching strategy using a concurrent dictionary for
    /// thread-safe asset storage and retrieval.
    /// </summary>
    public class DynamicCache : ICacheStrategy
    {
        private readonly ConcurrentDictionary<int, Object> m_Cache = 
            new ConcurrentDictionary<int, Object>();

        /// <summary>
        /// Gets the number of preloaded assets in the cache.
        /// Currently, this cache does not support preloading, so it always returns 0.
        /// </summary>
        public int PreloadedAssetCount => 0;

        /// <inheritdoc />
        public bool Get(IAssetPolicy policy, out Object result)
        {
            return m_Cache.TryGetValue(policy.Id, out result);
        }

        /// <inheritdoc />
        public void Add(IAssetPolicy policy, Object toAdd)
        {
            m_Cache.TryAdd(policy.Id, toAdd);
        }
        
        /// <inheritdoc />
        public void Clear()
        {
            m_Cache.Clear();
        }

        /// <summary>
        /// Retrieves the default asset for this cache strategy.
        /// Currently, this cache does not define a default asset, so it returns null.
        /// </summary>
        /// <returns>Null, as no default asset is defined.</returns>
        public Object GetDefault() 
        {
            return null;
        }
    }
}

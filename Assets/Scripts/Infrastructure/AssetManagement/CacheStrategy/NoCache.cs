using FormForge.AssetManagement.AssetPolicy;
using UnityEngine;

namespace FormForge.AssetManagement.CacheStrategy
{
    /// <summary>
    /// Implements a caching strategy that does not store any assets.
    /// Every request for an asset will result in a cache miss.
    /// </summary>
    public class NoCache : ICacheStrategy
    {
        /// <summary>
        /// Gets the number of preloaded assets in the cache.
        /// Since this cache strategy does not store assets, it always returns 0.
        /// </summary>
        public int PreloadedAssetCount => 0;

        /// <summary>
        /// Always returns false, as this strategy does not store assets.
        /// </summary>
        /// <param name="policy">The asset policy (ignored).</param>
        /// <param name="result">Always set to null.</param>
        /// <returns>False, indicating the asset is not cached.</returns>
        public bool Get(IAssetPolicy policy, out Object result)
        {
            result = default;
            return false;
        }

        /// <summary>
        /// Does nothing, as this strategy does not cache assets.
        /// </summary>
        /// <param name="policy">The asset policy (ignored).</param>
        /// <param name="toAdd">The asset to add (ignored).</param>
        public void Add(IAssetPolicy policy, Object toAdd) { }
        
        /// <summary>
        /// Does nothing, as there are no assets to clear.
        /// </summary>
        public void Clear() { }

        /// <summary>
        /// Retrieves the default asset for this cache strategy.
        /// Since this strategy does not cache assets, it always returns null.
        /// </summary>
        /// <returns>Null, as no default asset is defined.</returns>
        public Object GetDefault() 
        {
            return null;
        }
    }
}
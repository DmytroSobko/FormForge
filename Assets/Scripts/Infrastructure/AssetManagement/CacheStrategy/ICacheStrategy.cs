using FormForge.AssetManagement.AssetPolicy;
using UnityEngine;

namespace FormForge.AssetManagement.CacheStrategy
{
    /// <summary>
    /// Defines a caching strategy for asset management, allowing assets to be stored, retrieved, and cleared.
    /// </summary>
    public interface ICacheStrategy
    {
        /// <summary>
        /// Gets the count of preloaded assets in the cache.
        /// </summary>
        int PreloadedAssetCount { get; }

        /// <summary>
        /// Attempts to retrieve an asset from the cache based on the given asset policy.
        /// </summary>
        /// <param name="policy">The asset policy used to locate the cached asset.</param>
        /// <param name="result">The cached asset if found; otherwise, null.</param>
        /// <returns>True if the asset was found in the cache; otherwise, false.</returns>
        bool Get(IAssetPolicy policy, out Object result);

        /// <summary>
        /// Adds an asset to the cache based on the specified asset policy.
        /// </summary>
        /// <param name="policy">The asset policy used as a key for caching.</param>
        /// <param name="toAdd">The asset to add to the cache.</param>
        void Add(IAssetPolicy policy, Object toAdd);

        /// <summary>
        /// Clears all assets stored in the cache.
        /// </summary>
        void Clear();

        /// <summary>
        /// Retrieves the default asset for this cache strategy.
        /// </summary>
        /// <returns>The default asset, or null if no default is set.</returns>
        Object GetDefault();
    }
}
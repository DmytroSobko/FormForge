using System.Collections.Generic;
using Object = System.Object;

namespace FormForge.AssetManagement.AssetContext
{
    /// <summary>
    /// Represents a context for managing assets, allowing asset registration and release.
    /// </summary>
    public interface IAssetContext
    {
        /// <summary>
        /// Gets a collection of loaded assets.
        /// </summary>
        public IReadOnlyList<Object> LoadedAssets { get; }

        /// <summary>
        /// Registers a single asset into the context.
        /// </summary>
        /// <param name="asset">The asset to register.</param>
        void RegisterAsset(Object asset);
        
        /// <summary>
        /// Registers multiple assets into the context.
        /// </summary>
        /// <param name="assets">The list of assets to register.</param>
        void RegisterAssets(List<Object> assets);
        
        /// <summary>
        /// Unregisters a single asset from the context.
        /// </summary>
        /// <param name="asset">The asset to unregister.</param>
        void UnregisterAsset(Object asset);
        
        /// <summary>
        /// Unregisters multiple assets from the context.
        /// </summary>
        /// <param name="assets">The list of assets to unregister.</param>
        void UnregisterAssets(List<Object> assets);
        
        /// <summary>
        /// Checks if a specified asset is already registered in the context.
        /// </summary>
        /// <param name="asset">The asset to check for registration.</param>
        /// <returns>Returns a boolean indicating whether the asset is present in the context's loaded assets.</returns>
        bool HasAsset(Object asset);
        
        /// <summary>
        /// Clears all registered assets from the context.
        /// </summary>
        void Clear();
    }
}
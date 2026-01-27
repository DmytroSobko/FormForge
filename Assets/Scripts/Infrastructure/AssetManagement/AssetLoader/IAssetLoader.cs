using System.Threading.Tasks;
using FormForge.AssetManagement.AssetPolicy;
using FormForge.AssetManagement.CacheStrategy;
using UnityEngine;

namespace FormForge.AssetManagement.AssetLoader
{
    /// <summary>
    /// AssetLoader is responsible for loading, updating, and managing assets
    /// It provides asynchronous and not asynchronous methods to load assets,
    /// check for catalog updates, and manage the download and caching of assets.
    /// </summary>
    public interface IAssetLoader
    {
        /// <summary>
        /// Defines custom logging functionality for the AssetLoader to use.
        /// If not set, the AssetLoader will output through Unity's Debug class.
        /// </summary>
        void SetLogger(ILogger logger);

        /// <summary>
        /// Loads an asset synchronously using the specified asset policy and cache strategy.
        /// </summary>
        /// <typeparam name="TObject">The type of the asset to load.</typeparam>
        /// <param name="assetPolicy">The policy defining how the asset should be loaded.</param>
        /// <param name="cacheStrategy">The caching strategy to be applied.</param>
        /// <returns>The loaded asset.</returns>
        public TObject Load<TObject>(IAssetPolicy assetPolicy, ICacheStrategy cacheStrategy) 
            where TObject : Object;

        /// <summary>
        /// Loads an asset asynchronously using the specified asset policy and cache strategy.
        /// </summary>
        /// <typeparam name="TObject">The type of the asset to load.</typeparam>
        /// <param name="assetPolicy">The policy defining how the asset should be loaded.</param>
        /// <param name="cacheStrategy">The caching strategy to be applied.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the loaded asset.</returns>
        public Task<TObject> LoadAsync<TObject>(IAssetPolicy assetPolicy, ICacheStrategy cacheStrategy)
            where TObject : Object;

        /// <summary>
        /// Instantiates a GameObject using the specified asset policy and cache strategy at a given position and parent transform.
        /// </summary>
        /// <param name="assetPolicy">The policy defining how the asset should be instantiated.</param>
        /// <param name="cacheStrategy">The caching strategy to be applied.</param>
        /// <param name="position">The position at which to instantiate the GameObject.</param>
        /// <param name="parent">The parent transform for the instantiated GameObject.</param>
        /// <returns>The instantiated GameObject.</returns>
        public GameObject Instantiate(IAssetPolicy assetPolicy, ICacheStrategy cacheStrategy,
            Vector3 position, Transform parent);

        /// <summary>
        /// Instantiates a GameObject asynchronously using the specified asset policy and cache strategy at a given position and parent transform.
        /// </summary>
        /// <param name="assetPolicy">The policy defining how the asset should be instantiated.</param>
        /// <param name="cacheStrategy">The caching strategy to be applied.</param>
        /// <param name="position">The position at which to instantiate the GameObject.</param>
        /// <param name="parent">The parent transform for the instantiated GameObject.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the instantiated GameObject.</returns>
        public Task<GameObject> InstantiateAsync(IAssetPolicy assetPolicy, ICacheStrategy cacheStrategy,
            Vector3 position, Transform parent);
        
        /// <summary>
        /// Releases the specified asset.
        /// </summary>
        /// <typeparam name="TObject">The type of the asset to release.</typeparam>
        /// <param name="releaseObject">The asset to be released.</param>
        void Release<TObject>(TObject releaseObject) where TObject : Object;
    }
}

using System.Threading.Tasks;
using FormForge.AssetManagement.AssetContext;
using FormForge.AssetManagement.AssetPolicy;
using FormForge.AssetManagement.CacheStrategy;
using UnityEngine;

namespace FormForge.AssetManagement
{
    /// <summary>
    /// Manages asset loading, caching, and instantiation, supporting both synchronous and asynchronous operations.
    /// </summary>
    public interface IAssetManagementService
    {
        /// <summary>
        /// Defines custom logging functionality for the AssetManagementService to use.
        /// If not set, the AssetManagementService will output through Unity's Debug class.
        /// </summary>
        public void SetLogger(ILogger logger);
        
        /// <summary>
        /// Registers a caching strategy for a specified asset policy.
        /// </summary>
        /// <param name="policy">The asset policy associated with the caching strategy.</param>
        /// <param name="strategy">The caching strategy to use for the specified asset policy.</param>
        void RegisterStrategy(IAssetPolicy policy, ICacheStrategy strategy);

        /// <summary>
        /// Registers a new asset context of the specified type.
        /// If a context of this type already exists, a warning is logged, and no new context is created.
        /// </summary>
        /// <typeparam name="TContext">The type of the asset context to register.</typeparam>
        void RegisterContext<TContext>() where TContext : IAssetContext, new();
        
        /// <summary>
        /// Loads an asset synchronously based on the given asset policy.
        /// </summary>
        /// <typeparam name="TObject">The type of asset to load, must be a Unity Object.</typeparam>
        /// <param name="assetPolicy">The asset policy that defines how the asset should be loaded.</param>
        /// <returns>The loaded asset of type <typeparamref name="TObject"/>.</returns>
        TObject Load<TObject, TContext>(IAssetPolicy assetPolicy) 
            where TObject : Object 
            where TContext : IAssetContext, new();
        
        /// <summary>
        /// Loads an asset asynchronously based on the given asset policy.
        /// </summary>
        /// <typeparam name="TObject">The type of asset to load, must be a Unity Object.</typeparam>
        /// <param name="assetPolicy">The asset policy that defines how the asset should be loaded.</param>
        /// <returns>A task that resolves to the loaded asset of type <typeparamref name="TObject"/>.</returns>
        Task<TObject> LoadAsync<TObject, TContext>(IAssetPolicy assetPolicy)
            where TObject : Object
            where TContext : IAssetContext, new();
        
        /// <summary>
        /// Instantiates a GameObject from an asset and returns its requested component.
        /// </summary>
        /// <typeparam name="TComponent">The type of component to retrieve from the instantiated GameObject.</typeparam>
        /// <param name="assetPolicy">The asset policy that defines how the GameObject should be instantiated.</param>
        /// <param name="position">The position where the GameObject should be instantiated.</param>
        /// <param name="parent">The parent transform to attach the instantiated GameObject to (optional).</param>
        /// <returns>The instantiated component of type <typeparamref name="TComponent"/>.</returns>
        TComponent Instantiate<TComponent, TContext>(IAssetPolicy assetPolicy, Vector3 position, Transform parent = null)
            where TComponent : Component 
            where TContext : IAssetContext, new();
        
        /// <summary>
        /// Instantiates a GameObject asynchronously from an asset and returns its requested component.
        /// </summary>
        /// <typeparam name="TComponent">The type of component to retrieve from the instantiated GameObject.</typeparam>
        /// <param name="assetPolicy">The asset policy that defines how the GameObject should be instantiated.</param>
        /// <param name="position">The position where the GameObject should be instantiated.</param>
        /// <param name="parent">The parent transform to attach the instantiated GameObject to (optional).</param>
        /// <returns>A task that resolves to the instantiated component of type <typeparamref name="TComponent"/>.</returns>
        Task<TComponent> InstantiateAsync<TComponent, TContext>(IAssetPolicy assetPolicy, Vector3 position, Transform parent = null)
            where TComponent : Component 
            where TContext : IAssetContext, new();
        
        /// <summary>
        /// Releases a previously loaded asset. If the asset is a component, its associated GameObject is released.
        /// Logs a warning if the asset is null.
        /// </summary>
        /// <typeparam name="TObject">The type of asset to release, which must be a Unity Object.</typeparam>
        /// <param name="asset">The asset instance to be released.</param>
        void Release<TObject>(TObject asset) where TObject : Object;

        /// <summary>
        /// Releases all assets and instances within a specific context.
        /// </summary>
        /// <typeparam name="TContext">The asset context type from which assets will be released.</typeparam>
        void ReleaseContextAssets<TContext>() where TContext : IAssetContext, new();

        /// <summary>
        /// Releases all assets and instances across all contexts.
        /// </summary>
        public void ReleaseAllContextAssets();

        /// <summary>
        /// Clears all cached assets and instances.
        /// </summary>
        void ClearCache();
    }
}

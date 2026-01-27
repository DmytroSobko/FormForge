using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FormForge.AssetManagement.AssetContext;
using FormForge.AssetManagement.AssetLoader;
using FormForge.AssetManagement.AssetPolicy;
using FormForge.AssetManagement.CacheStrategy;
using UnityEngine;
using Object = UnityEngine.Object;

namespace FormForge.AssetManagement
{
    /// <inheritdoc />
    public class AssetManagementService : IAssetManagementService
    {
        public static bool RegisterContextsAutomatically = true;

        private ILogger m_logger;
        private readonly IAssetLoader m_assetLoader;

        private readonly Dictionary<int, ICacheStrategy> m_cacheStrategyMapping = 
            new Dictionary<int, ICacheStrategy>();
        private readonly Dictionary<Type, IAssetContext> m_assetContextMapping = 
            new Dictionary<Type, IAssetContext>();

        private readonly ICacheStrategy m_defaultAssetStrategy;
        private readonly ICacheStrategy m_defaultGameObjectStrategy;

        /// <summary>
        /// Initializes a new instance of the AssetManagementService.
        /// </summary>
        /// <param name="assetLoader">The asset loader responsible for loading and instantiating assets.</param>
        /// <param name="defaultAssetStrategy">The default caching strategy for assets.</param>
        /// <param name="defaultGameObjectStrategy">The default caching strategy for GameObjects.</param>
        public AssetManagementService(IAssetLoader assetLoader, ICacheStrategy defaultAssetStrategy,
            ICacheStrategy defaultGameObjectStrategy)
        {
            m_assetLoader = assetLoader;
            m_defaultAssetStrategy = defaultAssetStrategy;
            m_defaultGameObjectStrategy = defaultGameObjectStrategy;
        }
        
        public void SetLogger(ILogger logger)
        {
            m_logger = logger;
            m_assetLoader.SetLogger(logger);
        }
        
        public void RegisterStrategy(IAssetPolicy policy, ICacheStrategy strategy)
        {
            m_cacheStrategyMapping.Add(policy.Id, strategy);
        }
        
        public void RegisterContext<TContext>() where TContext : IAssetContext, new()
        {
            var contextType = typeof(TContext);
            if (m_assetContextMapping.TryGetValue(contextType, out var context))
            {
                LoggerHelper.LogWarning<AssetManagementService>($"Context of type {contextType.Name} already exists.", m_logger);
                return;
            }
            context = new TContext();
            m_assetContextMapping[contextType] = context;
            LoggerHelper.Log<AssetManagementService>($"Context of type {contextType.Name} has been registered.", m_logger);
        }

        public TObject Load<TObject, TContext>(IAssetPolicy assetPolicy) 
            where TObject : Object
            where TContext : IAssetContext, new()
        {
            ICacheStrategy strategy = GetStrategyForPolicyId(assetPolicy, m_defaultAssetStrategy);
            TObject loadedObject = m_assetLoader.Load<TObject>(assetPolicy, strategy);
            AddAssetToContext<TObject, TContext>(loadedObject);
            return loadedObject;
        }

        public async Task<TObject> LoadAsync<TObject, TContext>(IAssetPolicy assetPolicy) 
            where TObject : Object
            where TContext : IAssetContext, new()
        {
            ICacheStrategy strategy = GetStrategyForPolicyId(assetPolicy, m_defaultAssetStrategy);
            TObject loadedObject = await m_assetLoader.LoadAsync<TObject>(assetPolicy, strategy);
            AddAssetToContext<TObject, TContext>(loadedObject);
            return loadedObject;
        }

        public TComponent Instantiate<TComponent, TContext>(IAssetPolicy assetPolicy, Vector3 position, Transform parent = null) 
            where TComponent : Component
            where TContext : IAssetContext, new()
        {
            ICacheStrategy strategy = GetStrategyForPolicyId(assetPolicy, m_defaultGameObjectStrategy);
            GameObject go = m_assetLoader.Instantiate(assetPolicy, strategy, position, parent);
            TComponent component = go.GetComponent<TComponent>();
            AddAssetToContext<TComponent, TContext>(component);
            return component;
        }

        public async Task<TComponent> InstantiateAsync<TComponent, TContext>(IAssetPolicy assetPolicy, Vector3 position, Transform parent = null) 
            where TComponent : Component
            where TContext : IAssetContext, new()
        {
            ICacheStrategy strategy = GetStrategyForPolicyId(assetPolicy, m_defaultGameObjectStrategy);
            GameObject go = await m_assetLoader.InstantiateAsync(assetPolicy, strategy, position, parent);
            TComponent component = go.GetComponent<TComponent>();
            AddAssetToContext<TComponent, TContext>(component);
            return component;
        }
        
        public void Release<TObject>(TObject asset) where TObject : Object
        {
            if (asset == null)
            {
                LoggerHelper.LogWarning<AssetManagementService>("Attempted to release a null asset.", m_logger);
                return;
            }
            UnregisterAssetFromContexts(asset);
            m_assetLoader.Release(asset);
        }
        
        /// <summary>
        /// Retrieves the asset context for the specified type.
        /// If the context does not exist, a new instance is created and stored.
        /// </summary>
        /// <typeparam name="TContext">The type of asset context.</typeparam>
        /// <returns>The instance of the requested asset context.</returns>
        private IAssetContext GetContext<TContext>() where TContext : IAssetContext, new()
        {
            var contextType = typeof(TContext);
            if (m_assetContextMapping.TryGetValue(contextType, out var context))
            {
                return context;
            }

            if (!RegisterContextsAutomatically)
            {
                throw new InvalidOperationException($"Context of type {contextType.Name}" +
                                                    $" must be registered using {nameof(RegisterContext)} before accessing it.");
            }
           
            RegisterContext<TContext>();
            return m_assetContextMapping[contextType];
        }

        /// <summary>
        /// Adds a loaded asset to the corresponding asset context.
        /// </summary>
        /// <typeparam name="TObject">The type of asset being added.</typeparam>
        /// <typeparam name="TContext">The asset context type associated with the asset.</typeparam>
        /// <param name="asset">The asset to be added to the context.</param>
        private void AddAssetToContext<TObject, TContext>(TObject asset) 
            where TObject : Object
            where TContext : IAssetContext, new()
        {
            IAssetContext context = GetContext<TContext>();
            context.RegisterAsset(asset);
        }

        public void ReleaseContextAssets<TContext>() where TContext : IAssetContext, new()
        {
            IAssetContext context = GetContext<TContext>();
            foreach (Object asset in context.LoadedAssets)
            {
                m_assetLoader.Release(asset);
            }
            context.Clear();
            
            string logMsg =
                $"All assets from context {typeof(TContext).Name} have been released and the context has been cleared.";
            LoggerHelper.Log<AssetManagementService>(logMsg, m_logger);
        }

        public void ReleaseAllContextAssets()
        {
            foreach (var context in m_assetContextMapping.Values)
            {
                foreach (Object asset in context.LoadedAssets)
                {
                    m_assetLoader.Release(asset);
                } 
                context.Clear();
            }
        }

        /// <summary>
        /// Unregisters the asset from all contexts where it is registered.
        /// </summary>
        /// <param name="asset">The asset to unregister from the contexts.</param>
        private void UnregisterAssetFromContexts<TObject>(TObject asset) where TObject : Object
        {
            foreach (var context in m_assetContextMapping.Values)
            {
                if (!context.HasAsset(asset))
                {
                    continue;
                }
                
                context.UnregisterAsset(asset);
                
                string logMsg = $"Asset of type {asset.GetType().Name} removed from context {context.GetType().Name}.";
                LoggerHelper.Log<AssetManagementService>(logMsg, m_logger);
                break;
            }
        }

        public void ClearCache()
        {
            m_defaultAssetStrategy.Clear();
            m_defaultGameObjectStrategy.Clear();
            foreach (var strategyKvp in m_cacheStrategyMapping)
            {
                strategyKvp.Value.Clear();
            }
        }

        /// <summary>
        /// Retrieves the caching strategy associated with a given asset policy ID.
        /// If no specific strategy is found, the default strategy is used.
        /// </summary>
        /// <param name="policy">The asset policy for which to retrieve the caching strategy.</param>
        /// <param name="defaultStrategy">The default caching strategy to use if no specific strategy is found.</param>
        /// <returns>The caching strategy associated with the asset policy.</returns>
        private ICacheStrategy GetStrategyForPolicyId(IAssetPolicy policy, ICacheStrategy defaultStrategy)
        {
            if (!m_cacheStrategyMapping.TryGetValue(policy.Id, out ICacheStrategy strategy))
            {
                strategy = defaultStrategy;
            }
            return strategy;
        }
    }
}

using System.Collections.Generic;
using Object = System.Object;

namespace FormForge.AssetManagement.AssetContext
{
    /// <summary>
    /// Provides a base implementation of the IAssetContext interface.
    /// This class manages a collection of loaded assets and provides methods for asset registration and clearing.
    /// </summary>
    public abstract class BaseAssetContext : IAssetContext
    {
        public IReadOnlyList<Object> LoadedAssets => m_loadedAssets;
        private List<Object> m_loadedAssets = new List<Object>();
        
        /// <inheritdoc />
        public void RegisterAsset(Object asset)
        {
            m_loadedAssets.Add(asset);
        }
        
        /// <inheritdoc />
        public void RegisterAssets(List<Object> assets)
        {
            m_loadedAssets.AddRange(assets);
        }

        /// <inheritdoc />
        public void UnregisterAsset(Object asset)
        {
            m_loadedAssets.Remove(asset);
        }

        /// <inheritdoc />
        public void UnregisterAssets(List<Object> assets)
        {
            foreach (Object asset in assets)
            {
                m_loadedAssets.Remove(asset);
            }
        }

        /// <inheritdoc />
        public bool HasAsset(Object asset)
        {
            return m_loadedAssets.Contains(asset);
        }
        
        /// <inheritdoc />
        public void Clear()
        {
            m_loadedAssets.Clear();
        }
    }
}
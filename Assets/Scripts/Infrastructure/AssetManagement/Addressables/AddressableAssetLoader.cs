using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FormForge.AssetManagement.AssetPolicy;
using FormForge.AssetManagement.CacheStrategy;
using FormForge.AssetManagement.DownloadReporter;
using FormForge.Utils;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace FormForge.AssetManagement.Addressable.AssetLoader
{
    /// <inheritdoc />
    public class AddressableAssetLoader : IAddressableAssetLoader
    {
        private IAssetDownloadReporter m_downloadReporter;
        private ILogger m_logger;
        
        private List<IResourceLocator> m_catalogsLocators;
        private HashSet<string> m_addressableMap;
        
        public async Task InitializeAsync()
        {
            await InitializeAddressablesAsync();
            await UpdateCatalogsAsync();
            LoadMap();
        }
        
        public void SetLogger(ILogger logger)
        {
            m_logger = logger;
        }

        public void SetAssetDownloadReporter(IAssetDownloadReporter downloadReporter)
        {
            m_downloadReporter = downloadReporter;
        }

        /// <summary>
        /// Asynchronously initializes the addressables system.
        /// </summary>
        private async Task InitializeAddressablesAsync()
        {
            AsyncOperationHandle<IResourceLocator> initializeHandle = Addressables.InitializeAsync(false);
            await initializeHandle.Task;

            if (!IsHandleValid(initializeHandle, "Initialization"))
            {
                return;
            }

            Addressables.Release(initializeHandle);
        }
        
        /// <summary>
        /// Asynchronously checks for updates to the catalogs and updates them if necessary.
        /// </summary>
        private async Task UpdateCatalogsAsync()
        {
            AsyncOperationHandle<List<string>> updateCheckHandle = Addressables.CheckForCatalogUpdates(false);
            await updateCheckHandle.Task;

            if (!IsHandleValid(updateCheckHandle, "Catalog update check"))
            {
                return;
            }

            List<string> catalogsToUpdate = updateCheckHandle.Result;
            Addressables.Release(updateCheckHandle);

            if (catalogsToUpdate == null || catalogsToUpdate.Count == 0)
            {
                m_catalogsLocators = Addressables.ResourceLocators.ToList();
                return;
            }

            AsyncOperationHandle<List<IResourceLocator>> updateHandle = Addressables.UpdateCatalogs(catalogsToUpdate, false);
            await updateHandle.Task;

            if (!IsHandleValid(updateHandle, "Catalogs update"))
            {
                return;
            }

            m_catalogsLocators = updateHandle.Result;
            Addressables.Release(updateHandle);
        }

        /// <summary>
        /// Loads the addressable map by extracting keys from the catalog locators.
        /// </summary>
        private void LoadMap()
        {
            m_addressableMap = new HashSet<string>();
            foreach (IResourceLocator locator in m_catalogsLocators)
            {
                foreach (object item in locator.Keys)
                {
                    m_addressableMap.Add(item.ToString());
                }
            }
        }
        
        public async Task UpdateContentAsync()
        {
            if (m_catalogsLocators == null)
            {
                await UpdateCatalogsAsync();
            }

            IList<IResourceLocation> resourceLocations = await LoadResourceLocationsAsync(m_catalogsLocators);
            if (!HasResourceLocations(resourceLocations))
            {
                return;
            }
            await DownloadDependenciesAsync(resourceLocations);
        }
        
        /// <summary>
        /// Downloads the dependencies for a given list of resource locations.
        /// </summary>
        /// <param name="resourceLocations">A list of resource locations whose dependencies need to be downloaded.</param>
        private async Task DownloadDependenciesAsync(IList<IResourceLocation> resourceLocations)
        {
            AsyncOperationHandle downloadHandle = Addressables.DownloadDependenciesAsync(resourceLocations);
            await DownloadHelper.DownloadAsync(downloadHandle, m_downloadReporter);
            if (!IsHandleValid(downloadHandle, "Downloading catalog dependencies"))
            {
                return;
            }
            Addressables.Release(downloadHandle);
        }
        
        public async Task UpdateContentForLabelsAsync(params string[] labels)
        {
            foreach (string label in labels)
            {
                AsyncOperationHandle downloadHandle = Addressables.DownloadDependenciesAsync(label);
                await DownloadHelper.DownloadAsync(downloadHandle, m_downloadReporter);
                if (!IsHandleValid(downloadHandle, $"Downloading dependencies for label '{label}'"))
                {
                    return;
                }
                Addressables.Release(downloadHandle);
            }
        }
        
        public async Task<float> GetContentDownloadSizeAsync()
        {
            IList<IResourceLocation> resourceLocations = await LoadResourceLocationsAsync(m_catalogsLocators);
            if (!HasResourceLocations(resourceLocations))
            {
                return 0;
            }

            AsyncOperationHandle<long> getDownloadSizeHandle = Addressables.GetDownloadSizeAsync(resourceLocations);
            await getDownloadSizeHandle.Task;

            if (!IsHandleValid(getDownloadSizeHandle, "Get total download size"))
            {
                return 0;
            }

            long downloadSize = getDownloadSizeHandle.Result;
            Addressables.Release(getDownloadSizeHandle);
            return SizeConverter.BytesToMegabytes(downloadSize);
        }
        
        public async Task<float> GetDownloadSizeForLabelsAsync(params string[] labels)
        {
            long downloadSize = 0;
            foreach (var label in labels)
            {
                AsyncOperationHandle<long> getDownloadSizeHandle = Addressables.GetDownloadSizeAsync(label);
                await getDownloadSizeHandle.Task;

                if (!IsHandleValid(getDownloadSizeHandle, 
                        $"Get download size for label '{label}'"))
                {
                    return 0;
                }

                downloadSize += getDownloadSizeHandle.Result;
                string logMsg = $"Download size for label '{label}': " +
                             $"{SizeConverter.BytesToMegabytes(getDownloadSizeHandle.Result)} Mb.";
                LoggerHelper.Log<AddressableAssetLoader>(logMsg, m_logger);
                Addressables.Release(getDownloadSizeHandle);
            }
            return SizeConverter.BytesToMegabytes(downloadSize);
        }
        
        /// <summary>
        /// Loads resource locations based on a set of locators.
        /// </summary>
        /// <param name="locators">The locators from which to load resource locations.</param>
        /// <returns>A list of resource locations, or null if loading failed.</returns>
        private async Task<IList<IResourceLocation>> LoadResourceLocationsAsync(IEnumerable<IResourceLocator> locators)
        {
            IEnumerable<object> keysToCheck = locators.SelectMany(locator => locator.Keys);
            AsyncOperationHandle<IList<IResourceLocation>> loadLocationsHandle =
                Addressables.LoadResourceLocationsAsync(keysToCheck, Addressables.MergeMode.Union);
            await loadLocationsHandle.Task;

            if (!IsHandleValid(loadLocationsHandle, "Loading resource locations"))
            {
                return null;
            }

            IList<IResourceLocation> resourceLocations = loadLocationsHandle.Result;
            Addressables.Release(loadLocationsHandle);
            return resourceLocations;
        }
        
        public async Task<IResourceLocator> LoadContentCatalogAsync(string catalogPath)
        {
            if (string.IsNullOrEmpty(catalogPath))
            {
                LoggerHelper.LogError<AddressableAssetLoader>("Catalog path is null or empty.");
                return null;
            }

            AsyncOperationHandle<IResourceLocator> loadCatalogHandle =
                Addressables.LoadContentCatalogAsync(catalogPath, false);
            await loadCatalogHandle.Task;

            if (!IsHandleValid(loadCatalogHandle, $"Loading remote content catalog at {catalogPath}"))
            {
                return null;
            }

            IResourceLocator catalogLocator = loadCatalogHandle.Result;
            Addressables.Release(loadCatalogHandle);
            return catalogLocator;
        }
        
        public async Task<float> GetCatalogDownloadSizeAsync(IResourceLocator catalogLocator)
        {
            if (catalogLocator == null)
            {
                LoggerHelper.LogError<AddressableAssetLoader>("Catalog locator is null.");
                return 0;
            }
            
            IList<IResourceLocation> resourceLocations = await LoadResourceLocationsAsync(new[] { catalogLocator });
            if (!HasResourceLocations(resourceLocations))
            {
                return 0;
            }

            AsyncOperationHandle<long> getDownloadSizeHandle = Addressables.GetDownloadSizeAsync(resourceLocations);
            await getDownloadSizeHandle.Task;

            if (!IsHandleValid(getDownloadSizeHandle, "Getting catalog download size"))
            {
                return 0;
            }

            long downloadSize = getDownloadSizeHandle.Result;
            Addressables.Release(getDownloadSizeHandle);
            return SizeConverter.BytesToMegabytes(downloadSize);
        }
        
        public async Task DownloadCatalogDependenciesAsync(IResourceLocator catalogLocator)
        {
            if (catalogLocator == null)
            {
                LoggerHelper.LogError<AddressableAssetLoader>("Catalog locator is null.");
                return;
            }
            
            IList<IResourceLocation> resourceLocations = await LoadResourceLocationsAsync(new []{ catalogLocator });
            if (resourceLocations?.Count > 0)
            {
                await DownloadDependenciesAsync(resourceLocations);
            }
        }
        
        public async Task<TObject> LoadAsync<TObject>(IAssetPolicy assetPolicy, ICacheStrategy cacheStrategy)
            where TObject : Object
        {
            if (TryFetchFromCache(assetPolicy, cacheStrategy, out TObject result))
            {
                return result;
            }

            result = await Addressables.LoadAssetAsync<TObject>(assetPolicy.Address).Task;
            cacheStrategy.Add(assetPolicy, result);
            return result;
        }
        
        public TObject Load<TObject>(IAssetPolicy assetPolicy, ICacheStrategy cacheStrategy)
            where TObject : Object
        {
            if (TryFetchFromCache(assetPolicy, cacheStrategy, out TObject result))
            {
                return result;
            }

            result = Addressables.LoadAssetAsync<TObject>(assetPolicy.Address).WaitForCompletion();
            cacheStrategy.Add(assetPolicy, result);
            return result;
        }
        
        public GameObject Instantiate(IAssetPolicy assetPolicy, ICacheStrategy cacheStrategy,
            Vector3 position, Transform parent)
        {
            if (TryFetchFromCache(assetPolicy, cacheStrategy, out GameObject result))
            {
                return result;
            }

            result = Addressables.InstantiateAsync(assetPolicy.Address, position, Quaternion.identity, parent)
                .WaitForCompletion();
            cacheStrategy.Add(assetPolicy, result);
            return result;
        }
        
        public async Task<GameObject> InstantiateAsync(IAssetPolicy assetPolicy, ICacheStrategy cacheStrategy,
            Vector3 position, Transform parent)
        {
            if (TryFetchFromCache(assetPolicy, cacheStrategy, out GameObject result))
            {
                return result;
            }

            result = await Addressables.InstantiateAsync(assetPolicy.Address, position, Quaternion.identity, parent)
                .Task;
            cacheStrategy.Add(assetPolicy, result);
            return result;
        }
        
        public void Release<TObject>(TObject asset) where TObject : Object
        {
            if (asset is Component component)
            {
                LoggerHelper.Log<AssetManagementService>($"Releasing instance of {component.gameObject.name}.", m_logger);
                Addressables.ReleaseInstance(component.gameObject);
            }
            else
            {
                LoggerHelper.Log<AssetManagementService>($"Releasing asset of type {typeof(TObject).Name}.", m_logger);
                Addressables.Release(asset);
            }
        }

        /// <summary>
        /// Tries to fetch an asset from the cache based on the provided asset policy and cache strategy.
        /// <para>
        /// If the asset is not found in the addressable map, it returns the default value provided by the cache strategy.
        /// If the asset is found in the cache, it returns the cached asset.
        /// </para>
        /// </summary>
        /// <typeparam name="TObject">The type of object to fetch from the cache, which must be a subclass of Unity's Object.</typeparam>
        /// <param name="assetPolicy">The asset policy used to locate the asset, containing the address for the asset.</param>
        /// <param name="cacheStrategy">The caching strategy to use for fetching and providing default values if the asset is not found.</param>
        /// <param name="result">The result containing the fetched asset, or the default value if not found. Will be set to default if no asset is found.</param>
        /// <returns>
        /// <c>true</c> if the asset was successfully fetched from the cache, <c>false</c> otherwise.
        /// </returns>
        private bool TryFetchFromCache<TObject>(IAssetPolicy assetPolicy,
            ICacheStrategy cacheStrategy, out TObject result) where TObject : Object
        {
            if (!m_addressableMap.Contains(assetPolicy.Address))
            {
                result = (TObject)cacheStrategy.GetDefault();
                return true;
            }

            if (cacheStrategy.Get(assetPolicy, out Object resultObj))
            {
                result = (TObject)resultObj;
                return true;
            }
            
            result = default;
            return false;
        }
        
        /// <summary>
        /// Checks if the provided list of resource locations is valid and contains elements.
        /// Logs an error if the list is null or empty.
        /// </summary>
        /// <param name="resourceLocations">The list of resource locations to check.</param>
        /// <returns>True if the list contains resource locations; otherwise, false.</returns>
        private bool HasResourceLocations(IList<IResourceLocation> resourceLocations)
        {
            if (resourceLocations != null && resourceLocations.Count != 0)
            {
                return true;
            }
            LoggerHelper.LogError<AddressableAssetLoader>("No resource locations found.");
            return false;
        }
        
        /// <summary>
        /// Validates an AsyncOperationHandle to ensure it is valid and has completed successfully.
        /// Logs an error if the handle is invalid or if the operation fails.
        /// </summary>
        /// <param name="handle">The AsyncOperationHandle to validate.</param>
        /// <param name="operationName">The name of the operation being validated (used for logging).</param>
        /// <returns>True if the handle is valid and the operation succeeded; otherwise, false.</returns>
        private bool IsHandleValid(AsyncOperationHandle handle, string operationName)
        {
            if (!handle.IsValid())
            {
                LoggerHelper.LogError<AddressableAssetLoader>($"{operationName} handle is invalid.");
                return false;
            }

            if (handle.Status != AsyncOperationStatus.Succeeded)
            {
                LoggerHelper.LogError<AddressableAssetLoader>($"{operationName} failed with status: {handle.Status}");
                Addressables.Release(handle);
                return false;
            }

            LoggerHelper.Log<AddressableAssetLoader>($"{operationName} completed successfully.", m_logger);
            return true;
        }
    }
}

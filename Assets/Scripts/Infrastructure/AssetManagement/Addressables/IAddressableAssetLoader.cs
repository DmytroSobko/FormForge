using System.Threading.Tasks;
using FormForge.AssetManagement.AssetLoader;
using FormForge.AssetManagement.DownloadReporter;
using UnityEngine.AddressableAssets.ResourceLocators;

namespace FormForge.AssetManagement.Addressable.AssetLoader
{
    /// <summary>
    /// AddressableAssetLoader is responsible for loading, updating, and managing addressable assets
    /// using Unity's Addressables system. It provides asynchronous and not asynchronous methods to load assets,
    /// check for catalog updates, and manage the download and caching of assets.
    /// </summary>
    public interface IAddressableAssetLoader : IAssetLoader
    {
        /// <summary>
        /// Sets a custom asset download reporter for the AssetLoader to use.
        /// If not provided, no download reporting will be done during asset downloads.
        /// </summary>
        void SetAssetDownloadReporter(IAssetDownloadReporter downloadReporter);
        
        /// <summary>
        /// Initializes the addressable asset loader, setting up necessary resources and catalogs.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task InitializeAsync();

        /// <summary>
        /// Asynchronously updates content that has been changed and requires to be updated.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task UpdateContentAsync();

        /// <summary>
        /// Updates content asynchronously based on the provided labels.
        /// </summary>
        /// <param name="labels">An array of labels used to filter the content to update.</param>
        Task UpdateContentForLabelsAsync(params string[] labels);

        /// <summary>
        /// Retrieves the total download size of all assets that require to be updated.
        /// </summary>
        /// <returns>A task representing the asynchronous operation, returning the total download size in bytes.</returns>
        Task<float> GetContentDownloadSizeAsync();

        /// <summary>
        /// Retrieves the download size of assets filtered by the provided labels.
        /// </summary>
        /// <param name="labels">An array of labels used to filter assets for download size calculation.</param>
        /// <returns>A task representing the asynchronous operation, returning the total download size in bytes for the given labels.</returns>
        Task<float> GetDownloadSizeForLabelsAsync(params string[] labels);
        
        /// <summary>
        /// Loads a content catalog from the specified path and returns an asynchronous task.
        /// The catalog contains metadata about addressable assets, including locations and dependencies.
        /// </summary>
        /// <param name="catalogPath">The path to the content catalog to load.</param>
        /// <returns>A task representing the asynchronous operation that returns an IResourceLocator.</returns>
        Task<IResourceLocator> LoadContentCatalogAsync(string catalogPath);

        /// <summary>
        /// Asynchronously retrieves the total download size of the assets in the specified catalog.
        /// This method calculates the size of all assets contained within the catalog, which may be useful
        /// for checking how much data needs to be downloaded before loading assets.
        /// </summary>
        /// <param name="catalogLocator">The IResourceLocator that represents the loaded content catalog.</param>
        /// <returns>A task that represents the asynchronous operation, with a float result indicating the total download size in bytes.</returns>
        Task<float> GetCatalogDownloadSizeAsync(IResourceLocator catalogLocator);

        /// <summary>
        /// Asynchronously downloads the dependencies for the specified catalog locator.
        /// </summary>
        /// <param name="catalogLocator">The locator for the catalog whose dependencies are to be downloaded.</param>
        Task DownloadCatalogDependenciesAsync(IResourceLocator catalogLocator);
    }
}
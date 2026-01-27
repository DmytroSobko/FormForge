using System.Threading.Tasks;
using FormForge.AssetManagement.DownloadReporter;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace FormForge.AssetManagement
{
    public static class DownloadHelper
    {
        /// <summary>
        /// Asynchronously monitors and reports the download progress of an asset.
        /// </summary>
        /// <param name="handle">The async operation handle for the asset download.</param>
        /// <param name="downloadReporter">An optional reporter to track and report the download progress.</param>
        /// <returns>A task that completes when the download operation is finished.</returns>
        public static async Task DownloadAsync(AsyncOperationHandle handle, IAssetDownloadReporter downloadReporter = null)
        {
            while (!handle.IsDone && handle.IsValid())
            {
                await Task.Yield();
                downloadReporter?.Report(handle.GetDownloadStatus().Percent);
            }
    
            if (handle.IsValid())
            {
                downloadReporter?.Report(1f);
            }
    
            downloadReporter?.Reset();
        }
    }
}

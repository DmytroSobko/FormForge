using System;

namespace FormForge.AssetManagement.DownloadReporter
{
    /// <summary>
    /// AssetDownloadReporter is responsible for reporting and tracking the progress of asset downloads.
    /// Provides a progress value and an event for notifying updates.
    /// </summary>
    public interface IAssetDownloadReporter : IProgress<float>
    {
        /// <summary>
        /// Gets the current download progress.
        /// </summary>
        float Progress { get; }

        /// <summary>
        /// Event triggered when the progress is updated.
        /// </summary>
        event Action ProgressUpdated;

        /// <summary>
        /// Reports the current progress value.
        /// </summary>
        /// <param name="value">The progress value.</param>
        void Report(float value);

        /// <summary>
        /// Resets the progress to zero.
        /// </summary>
        void Reset();
    }
}
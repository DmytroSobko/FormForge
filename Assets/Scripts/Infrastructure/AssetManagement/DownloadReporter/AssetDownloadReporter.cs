using System;

namespace FormForge.AssetManagement.DownloadReporter
{
    /// <inheritdoc />
    public class AssetDownloadReporter : IAssetDownloadReporter
    {
        public float Progress { get; private set; }
        
        public event Action ProgressUpdated;
        
        public void Report(float value)
        {
            Progress = value;
            ProgressUpdated?.Invoke();
        }
        
        public void Reset()
        {
            Progress = 0;
            ProgressUpdated = null;
        }
    }
}
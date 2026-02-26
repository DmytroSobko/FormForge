namespace FormForge.Infrastructure.UI.Overlays.LoadingOverlay.Messages
{
    public class LoadingOverlaySetProgressMessage
    {
        public float Progress { get; }

        public LoadingOverlaySetProgressMessage(float progress)
        {
            Progress = progress;
        }
    }
}
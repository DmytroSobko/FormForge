namespace FormForge.Infrastructure.UI.LoadingOverlay.Messages
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
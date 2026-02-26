namespace FormForge.Infrastructure.UI.Overlays.ErrorOverlay.Messages
{
    public class ErrorOverlayShowMessage
    {
        public string Error { get; }
       
        public ErrorOverlayShowMessage(string error)
        {
            Error = error;
        }
    }
}
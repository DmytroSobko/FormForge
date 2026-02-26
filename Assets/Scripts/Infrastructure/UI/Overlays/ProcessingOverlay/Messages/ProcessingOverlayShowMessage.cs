namespace FormForge.Infrastructure.UI.Overlays.ProcessingOverlay.Messages
{
    public class ProcessingOverlayShowMessage
    {
        public string Process { get; }
       
        public ProcessingOverlayShowMessage(string process)
        {
            Process = process;
        }
    }
}
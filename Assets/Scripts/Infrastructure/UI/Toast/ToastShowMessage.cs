namespace FormForge.Infrastructure.UI.Toast
{
    public class ToastShowMessage
    {
        public string Toast { get; }
        public ToastType Type { get; }
        public float Duration { get; }

        public ToastShowMessage(string toast, ToastType type = ToastType.Info, float duration = 2.5f)
        {
            Toast = toast;
            Type = type;
            Duration = duration;
        }
    }
}
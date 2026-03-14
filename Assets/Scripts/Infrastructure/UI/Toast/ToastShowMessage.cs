namespace FormForge.Infrastructure.UI.Toast
{
    public class ToastShowMessage
    {
        public string Toast { get; }
        public EToastType Type { get; }
        public float Duration { get; }

        public ToastShowMessage(string toast, EToastType type = EToastType.Info, float duration = 2.5f)
        {
            Toast = toast;
            Type = type;
            Duration = duration;
        }
    }
}
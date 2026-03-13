namespace Infrastructure.Services.ToastService
{
    public interface IToastService
    {
        void Show(string toast);
        void Success(string toast);
        void Error(string toast);
        void Warning(string toast);
    }
}
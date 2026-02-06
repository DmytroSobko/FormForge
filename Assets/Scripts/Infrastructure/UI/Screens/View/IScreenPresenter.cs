using System.Threading.Tasks;
using FormForge.Infrastructure.UI.Screens.Model;

namespace FormForge.Infrastructure.UI.Screens.View
{
    public interface IScreenPresenter<in TScreenViewModel>: IPresenter
        where TScreenViewModel: IScreenViewModel
    {
        IScreenViewModel ViewModel { get; set; }
        State ScreenState { get; }
        string ScreenId { get; }
        bool IsInitialized { get; }
        bool IsConfigured { get; }
        bool IsFocused { get; }
        bool IsOpen { get; }
        bool KeepScreenOpened { get; }

        Task Configure(TScreenViewModel viewModel);
        Task Initialize();
        void Open();
        void GetFocus();
        void Refresh();
        void LoseFocus();
        void CloseInternal();
        void Dispose();
    }
}
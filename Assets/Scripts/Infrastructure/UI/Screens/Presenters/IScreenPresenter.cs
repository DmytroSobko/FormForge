using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using FormForge.Infrastructure.UI.Screens.Models;

namespace FormForge.Infrastructure.UI.Screens.Presenters
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

        UniTask Configure(TScreenViewModel viewModel);
        UniTask Initialize();
        
        void Open();
        void GetFocus();
        void Refresh();
        void LoseFocus();
        void CloseInternal();
        void Dispose();
    }
}
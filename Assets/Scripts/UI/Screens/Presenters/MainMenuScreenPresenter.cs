using System.Threading.Tasks;
using FormForge.Infrastructure.UI.Screens.Model;
using FormForge.Infrastructure.UI.Screens.View;
using FormForge.UI.Screens.ViewModels;
using FormForge.UI.Screens.Views;
using UnityEngine;

namespace FormForge.UI.Screens.Presenters
{
    public class MainMenuScreenPresenter : ScreenPresenter
    {
        [SerializeField] private MainMenuScreenView m_View;

        public override Task Configure(IScreenViewModel viewModel)
        {
            ViewModel = (MainMenuViewModel) viewModel;
            
            return base.Configure(viewModel);
        }

        public override Task Initialize()
        {
            return base.Initialize();
        }
    }
}
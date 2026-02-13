using System.Threading.Tasks;
using FormForge.Infrastructure.UI.Screens.Models;
using FormForge.Infrastructure.UI.Screens.Presenters;
using FormForge.UI.Screens.Models;
using FormForge.UI.Screens.Views;
using UnityEngine;

namespace FormForge.UI.Screens.Presenters
{
    public class AthletesScreenPresenter : ScreenPresenter
    {
        [SerializeField] private AthletesScreenView m_View;

        public override Task Configure(IScreenViewModel viewModel)
        {
            ViewModel = (AthletesScreenViewModel) viewModel;
            
            return base.Configure(viewModel);
        }

        public override Task Initialize()
        {
            return base.Initialize();
        }
    }
}
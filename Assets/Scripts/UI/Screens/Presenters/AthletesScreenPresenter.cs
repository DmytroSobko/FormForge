using System.Threading.Tasks;
using FormForge.Infrastructure.UI.Screens.Models;
using FormForge.Infrastructure.UI.Screens.Presenters;
using FormForge.UI.Screens.Models;
using FormForge.UI.Screens.Views.AthleteScreen;
using UnityEngine;

namespace FormForge.UI.Screens.Presenters
{
    public class AthletesScreenPresenter : ScreenPresenter
    {
        private const string k_NoContentMessage = "No athletes have been created yet.";

        private AthletesScreenViewModel TypedViewModel => (AthletesScreenViewModel) ViewModel;
        
        [SerializeField] private AthletesScreenView m_View;

        public override async Task Configure(IScreenViewModel viewModel)
        {
            await base.Configure(viewModel);

            AthletesDataProvider dataProvider = new AthletesDataProvider(TypedViewModel.Athletes);
            
            m_View.InitView(dataProvider, k_NoContentMessage);
        }
    }
}
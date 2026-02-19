using FormForge.Domain.Athletes;
using FormForge.Infrastructure.Services;
using FormForge.Infrastructure.UI.Screens.Presenters;
using FormForge.Infrastructure.UI.Screens.ViewModels;
using FormForge.Infrastructure.UI.Selection;
using FormForge.Networking.Athletes.Requests;
using FormForge.Services.AthletesService;
using FormForge.UI.Screens.ViewModels.CreateAthleteScreen;
using FormForge.UI.Screens.Views.CreateAthleteScreen;
using UnityEngine;

namespace FormForge.UI.Screens.Presenters.CreateAthleteScreen
{
    public class CreateAthleteScreenPresenter : ScreenPresenter
    {
        private CreateAthleteScreenViewModel TypedViewModel => (CreateAthleteScreenViewModel) ViewModel;
        
        [SerializeField] private CreateAthleteScreenView m_View;
        
        private IAthletesService m_AthletesService;
        
        private readonly SingleSelectionController<AthleteTypeItemViewModel> m_SelectionController 
            = new SingleSelectionController<AthleteTypeItemViewModel>();

        protected override void OnInitialize()
        {
            m_AthletesService = ServiceLocator.GetService<IAthletesService>();
            
            AddListeners();
            
            base.OnInitialize();
        }
        
        protected override void OnConfigure(IScreenViewModel viewModel)
        {
            m_View.Bind(TypedViewModel);
            
            foreach (var athlete in TypedViewModel.AthleteTypes.Values)
            {
                GameObject athleteTypeItem = m_View.CreateAthleteTypeItem();
                AthleteTypeItemPresenter athleteTypeItemPresenter = 
                    athleteTypeItem.GetComponent<AthleteTypeItemPresenter>();

                Sprite athleteIcon = 
                    TypedViewModel.AthleteTypeVisualsDatabase.Get(athlete.Type).Icon;
                
                AthleteTypeItemViewModel itemViewModel = new AthleteTypeItemViewModel(athlete, athleteIcon);
                athleteTypeItemPresenter.Initialize(itemViewModel);

                m_SelectionController.Register(athleteTypeItemPresenter);
            }
            
            base.OnConfigure(viewModel);
        }
        
        private void AddListeners()
        {
            m_SelectionController.OnSelectionChanged += OnAthleteSelected;
            m_View.OnCreateClicked += OnCreateClicked;
        }
        
        private void RemoveListeners()
        {
            m_SelectionController.OnSelectionChanged -= OnAthleteSelected;
            m_View.OnCreateClicked -= OnCreateClicked;
        }

        protected override void OnDispose()
        {
            RemoveListeners();
            base.OnDispose();
        }

        private async void OnCreateClicked(string athleteName)
        {
            EAthleteType athleteType = m_SelectionController.SelectedValue.AthleteTypeConfig.Type;
            
            Athlete response = await m_AthletesService.CreateAthlete(athleteType, athleteName);
            
            
        }
        
        private void OnAthleteSelected(AthleteTypeItemViewModel viewModel)
        {
            Debug.Log($"Selected: {viewModel.AthleteTypeConfig}");
        }
    }
}
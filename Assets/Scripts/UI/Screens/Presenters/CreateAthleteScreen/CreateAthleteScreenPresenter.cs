using FormForge.Domain.Athletes;
using FormForge.Helpers;
using FormForge.Infrastructure.Logging;
using FormForge.Infrastructure.Services;
using FormForge.Infrastructure.Services.HttpClientService;
using FormForge.Infrastructure.Services.MessageService.Interfaces;
using FormForge.Infrastructure.Services.ToastService;
using FormForge.Infrastructure.UI.Overlays.ErrorOverlay.Messages;
using FormForge.Infrastructure.UI.Overlays.ProcessingOverlay.Messages;
using FormForge.Infrastructure.UI.Screens.Presenters;
using FormForge.Infrastructure.UI.Screens.ViewModels;
using FormForge.Infrastructure.UI.Selection;
using FormForge.Services.AthletesService;
using FormForge.Services.ConfigsService;
using FormForge.Services.VisualsService;
using FormForge.Statics;
using FormForge.UI.Screens.ViewModels.CreateAthleteScreen;
using FormForge.UI.Screens.Views.CreateAthleteScreen;
using UnityEngine;

namespace FormForge.UI.Screens.Presenters.CreateAthleteScreen
{
    public class CreateAthleteScreenPresenter : ScreenPresenter
    {
        private CreateAthleteScreenViewModel TypedViewModel => (CreateAthleteScreenViewModel) ViewModel;
        
        [SerializeField] private CreateAthleteScreenView m_View;

        private readonly SingleSelectionController<AthleteTypeItemViewModel> m_SelectionController 
            = new SingleSelectionController<AthleteTypeItemViewModel>();

        private UnityLogger m_Logger = new UnityLogger(nameof(CreateAthleteScreenPresenter));
        
        private IMessageService m_MessageService;
        private IAthletesService m_AthletesService;
        private IToastService m_ToastService;
        private IVisualsService m_VisualsService;
        private IConfigsService m_ConfigsService;
        
        protected override void OnInitialize()
        {
            m_MessageService = ServiceLocator.GetService<IMessageService>();
            m_AthletesService = ServiceLocator.GetService<IAthletesService>();
            m_ToastService = ServiceLocator.GetService<IToastService>();
            m_VisualsService = ServiceLocator.GetService<IVisualsService>();
            m_ConfigsService = ServiceLocator.GetService<IConfigsService>();
            
            AddListeners();
            
            base.OnInitialize();
        }
        
        protected override void OnConfigure(IScreenViewModel viewModel)
        {
            m_View.Bind(TypedViewModel);
            
            foreach (var athlete in m_ConfigsService.AthleteTypes.Values)
            {
                GameObject athleteTypeItem = m_View.CreateAthleteTypeItem();
                AthleteTypeItemPresenter athleteTypeItemPresenter = 
                    athleteTypeItem.GetComponent<AthleteTypeItemPresenter>();

                Sprite athleteIcon = m_VisualsService.GetAthleteTypeVisuals(athlete.Type).Icon;
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
            var error = ValidationHelper.ValidateAthleteName(athleteName);
            if (error != null)
            {
                m_ToastService.Error(error);
                return;
            }
            
            if (m_SelectionController.SelectedValue == null)
            {
                m_ToastService.Error(UIStrings.CreateAthlete.SelectAthleteType);
                return;
            }
            
            EAthleteType athleteType = m_SelectionController.SelectedValue.AthleteTypeConfig.Type;
            m_View.SetInteractable(false);
            m_MessageService.Send(new ProcessingOverlayShowMessage(UIStrings.CreateAthlete.Creating));

            try
            { 
                await m_AthletesService.CreateAthlete(athleteType, athleteName);
            }
            catch (ApiException e)
            {
                m_Logger?.LogError($"API Error {e.StatusCode}: {e.ErrorCode} - {e.Message}");
                
                string errorMessage = string.Format(UIStrings.CreateAthlete.FailedWithError, e.StatusCode, e.Message);
                m_MessageService.Send(new ErrorOverlayShowMessage(errorMessage));
            }
            finally
            {
                m_View.SetInteractable(true);
                m_MessageService.Send(new ProcessingOverlayHideMessage());
                m_ToastService.Success(UIStrings.CreateAthlete.Toast.Success(athleteName));
            }
        }
        
        private void OnAthleteSelected(AthleteTypeItemViewModel viewModel)
        {
            m_Logger?.Log($"Selected: {viewModel.AthleteTypeConfig.Type}");
        }
    }
}
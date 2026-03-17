using FormForge.Infrastructure.Services;
using FormForge.Infrastructure.Services.MessageService.Interfaces;
using FormForge.Infrastructure.UI.Screens.Presenters;
using FormForge.Infrastructure.UI.Screens.ViewModels;
using FormForge.UI.FrontendStateMachine;
using FormForge.UI.FrontendStateMachine.Messages;
using FormForge.UI.Screens.ViewModels.TrainingPlansScreen;
using FormForge.UI.Screens.Views.TrainingPlansScreen;
using UnityEngine;

namespace FormForge.UI.Screens.Presenters.TrainingPlansScreen
{
    public class TrainingPlansScreenPresenter : ScreenPresenter
    {
        private TrainingPlansScreenViewModel TypedViewModel => (TrainingPlansScreenViewModel) ViewModel;
        
        [SerializeField] private TrainingPlansScreenView m_View;
        private IMessageService m_MessageService;

        protected override void OnInitialize()
        {
            m_MessageService = ServiceLocator.GetService<IMessageService>();

            AddListeners();
            
            base.OnInitialize();
        }

        protected override void OnConfigure(IScreenViewModel viewModel)
        {
            m_View.Bind(TypedViewModel);

            base.OnConfigure(viewModel);
        }

        private void AddListeners()
        {
            m_View.CreateButtonClicked += OnCreateClicked;
        }
        
        private void RemoveListeners()
        {
            m_View.CreateButtonClicked -= OnCreateClicked;
        }

        protected override void OnDispose()
        {
            RemoveListeners();
            base.OnDispose();
        }

        private void OnCreateClicked()
        {
            m_MessageService.Send(new SwitchFrontendStateMessage(FrontendStates.CreateTrainingPlanScreen));
        }
    }
}
using System.Collections.Generic;
using FormForge.Domain.Athletes;
using FormForge.Domain.Exercises;
using FormForge.Domain.TrainingPlans;
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
using FormForge.Services.AthletesService;
using FormForge.Services.ConfigsService;
using FormForge.Services.TrainingPlansService;
using FormForge.Services.VisualsService;
using FormForge.Statics;
using FormForge.UI.Screens.ViewModels.CreateTrainingPlanScreen;
using FormForge.UI.Screens.Views.CreateTrainingPlanScreen;
using UnityEngine;

namespace FormForge.UI.Screens.Presenters.CreateTrainingPlanScreen
{
    public class CreateTrainingPlanScreenPresenter : ScreenPresenter
    {
        private CreateTrainingPlanScreenViewModel TypedViewModel => (CreateTrainingPlanScreenViewModel) ViewModel;
        
        [SerializeField] private CreateTrainingPlanScreenView m_View;
        [SerializeField] private List<TrainingPlanDayPresenter> m_TrainingDayPresenters;

        private Dictionary<ETrainingDayOfWeek, TrainingPlanDayPresenter> m_TrainingDaysLookup;
        
        private UnityLogger m_Logger = new UnityLogger(nameof(CreateTrainingPlanScreenPresenter));
        
        private IMessageService m_MessageService;       
        private ITrainingPlansService m_TrainingPlansService;
        private IToastService m_ToastService;
        private IVisualsService m_VisualsService;
        private IConfigsService m_ConfigsService;
        
         protected override void OnInitialize()
        {
            m_MessageService = ServiceLocator.GetService<IMessageService>();
            m_TrainingPlansService = ServiceLocator.GetService<ITrainingPlansService>();
            m_ToastService = ServiceLocator.GetService<IToastService>();

            AddListeners();
            InitLookup();
            
            base.OnInitialize();
        }

         private void InitLookup()
         {
             m_TrainingDaysLookup = new Dictionary<ETrainingDayOfWeek, TrainingPlanDayPresenter>();
             foreach (var presenter in m_TrainingDayPresenters)
             {
                 presenter.Initialize();
                 m_TrainingDaysLookup[presenter.DayOfWeek] = presenter;
             }
         }
        
        protected override void OnConfigure(IScreenViewModel viewModel)
        {
            m_View.Bind(TypedViewModel);
            
            base.OnConfigure(viewModel);
        }
        
        private void AddListeners()
        {
            m_View.OnCreateClicked += OnCreateClicked;
        }

        private TrainingPlan BuildTrainingPlan(string trainingPlanName)
        {
            var trainingPlan = new TrainingPlan(trainingPlanName);

            foreach (TrainingPlanDayPresenter dayPresenter in m_TrainingDayPresenters)
            {
                var trainingDay = new TrainingDay(dayPresenter.DayOfWeek, dayPresenter.PlannedExercises);
                trainingPlan.AddDay(trainingDay);
            }
            return trainingPlan;
        }
        
        private void RemoveListeners()
        {
            m_View.OnCreateClicked -= OnCreateClicked;
        }

        protected override void OnDispose()
        {
            RemoveListeners();
            base.OnDispose();
        }

        private async void OnCreateClicked(string trainingPlanName)
        {
            var error = ValidationHelper.ValidateTrainingPlanName(trainingPlanName);
            if (error != null)
            {
                m_ToastService.Error(error);
                return;
            }

            TrainingPlan trainingPlan = BuildTrainingPlan(trainingPlanName);

            // EAthleteType athleteType = m_SelectionController.SelectedValue.AthleteTypeConfig.Type;
            // m_View.SetInteractable(false);
            // m_MessageService.Send(new ProcessingOverlayShowMessage(UIStrings.CreateAthlete.Creating));
            //
            // try
            // { 
            //     await m_AthletesService.CreateAthlete(athleteType, athleteName);
            // }
            // catch (ApiException e)
            // {
            //     m_Logger?.LogError($"API Error {e.StatusCode}: {e.ErrorCode} - {e.Message}");
            //     
            //     string errorMessage = string.Format(UIStrings.CreateAthlete.FailedWithError, e.StatusCode, e.Message);
            //     m_MessageService.Send(new ErrorOverlayShowMessage(errorMessage));
            // }
            // finally
            // {
            //     m_View.SetInteractable(true);
            //     m_MessageService.Send(new ProcessingOverlayHideMessage());
            //     m_ToastService.Success(UIStrings.CreateAthlete.Toast.Success(athleteName));
            // }
        }
    }
}
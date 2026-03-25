using System;
using System.Collections.Generic;
using FormForge.Domain.Exercises;
using FormForge.Domain.TrainingPlans;
using FormForge.Infrastructure.Services;
using FormForge.Infrastructure.Services.MessageService.Interfaces;
using FormForge.Infrastructure.UI;
using FormForge.UI.Screens.Messages;
using FormForge.UI.Screens.Views.CreateTrainingPlanScreen;
using UnityEngine;

namespace FormForge.UI.Screens.Presenters.CreateTrainingPlanScreen
{
    public class TrainingPlanDayPresenter : MonoBehaviour,
        IPresenter, IMessageReceiver<ExerciseAddedToPlanMessage>, IDisposable
    {
        [SerializeField] private ETrainingDayOfWeek m_DayOfWeek;
        [SerializeField] private TrainingPlanDayView m_TrainingPlanDayView;

        private List<PlannedExercise> m_PlannedExercises = new List<PlannedExercise>();
        
        public ETrainingDayOfWeek DayOfWeek => m_DayOfWeek;
        public List<PlannedExercise> PlannedExercises => m_PlannedExercises;

        public IMessageService m_MessageService;
        public void Initialize()
        {
            m_MessageService = ServiceLocator.GetService<IMessageService>();
            m_MessageService.Register(this);
            
            m_TrainingPlanDayView.AddButtonClicked += OnAddButtonClicked;
        }

        public void Dispose()
        {
            m_MessageService?.Unregister(this);
        }
        
        private void OnAddButtonClicked()
        {
            // show select exercise popup and pass m_PlannedExercises
        }

        public void HandleMessage(ExerciseAddedToPlanMessage messageData = null)
        {
            if (messageData == null)
            {
                return;
            }
            
            xZx
        }
    }
}
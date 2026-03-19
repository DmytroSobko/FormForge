using System.Collections.Generic;
using FormForge.Domain.Exercises;
using FormForge.Domain.TrainingPlans;
using FormForge.Infrastructure.UI;
using FormForge.UI.Screens.Views.CreateTrainingPlanScreen;
using UnityEngine;

namespace FormForge.UI.Screens.Presenters.CreateTrainingPlanScreen
{
    public class TrainingPlanDayPresenter : MonoBehaviour, IPresenter
    {
        [SerializeField] private ETrainingDayOfWeek m_DayOfWeek;
        [SerializeField] private TrainingPlanDayView m_TrainingPlanDayView;

        private List<PlannedExercise> m_PlannedExercises = new List<PlannedExercise>();
        
        public ETrainingDayOfWeek DayOfWeek => m_DayOfWeek;
        public List<PlannedExercise> PlannedExercises => m_PlannedExercises;

        public void Initialize()
        {
            m_TrainingPlanDayView.AddButtonClicked += OnAddButtonClicked;
        }
        
        private void OnAddButtonClicked()
        {
            // show select exercise popup and pass m_PlannedExercises
        }
    }
}
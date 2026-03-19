using System;
using System.Collections.Generic;
using FormForge.Infrastructure.Collections;
using FormForge.Infrastructure.UI.Screens.Views;
using FormForge.UI.Screens.ViewModels.CreateTrainingPlanScreen;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FormForge.UI.Screens.Views.CreateTrainingPlanScreen
{
    public class CreateTrainingPlanScreenView : BaseScreenView
    {
        [SerializeField] private List<TrainingPlanDayView> m_TraininDayViews;
        [SerializeField] private TMP_InputField m_TrainingPlanName;
        [SerializeField] private Button m_CreateButton;
        [SerializeField] private int m_ExercisePoolSize;

        private Action m_OnCreateClicked;
        
        public event Action<string> OnCreateClicked;
        
        private void Awake()
        {
            m_CreateButton.onClick.AddListener(OnCreateButtonClicked);
        }

        private void OnDestroy()
        {
            m_CreateButton.onClick.RemoveListener(OnCreateButtonClicked);
        }
        
        public void Bind(CreateTrainingPlanScreenViewModel viewModel)
        {
            var exerciseItemsPool = new Pool<PoolableObject>(m_ExercisePoolSize, viewModel.ExerciseItemPrefab);

            foreach (TrainingPlanDayView trainingDayView in m_TraininDayViews)
            {
                trainingDayView.Init(exerciseItemsPool);
            }
        }

        private void OnCreateButtonClicked()
        {
            OnCreateClicked?.Invoke(m_TrainingPlanName.text);
        }
    }
}